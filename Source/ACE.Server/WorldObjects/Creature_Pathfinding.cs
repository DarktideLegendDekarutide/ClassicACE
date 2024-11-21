using ACE.Common;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Server.Entity;
using ACE.Server.Entity.Actions;
using ACE.Server.Factories;
using ACE.Server.Managers;
using ACE.Server.Physics.Animation;
using ACE.Server.Physics.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace ACE.Server.WorldObjects
{
    partial class Creature
    {
        private PathfindingState PathfindingState = new PathfindingState();

        public bool IsPatrolCreature => PathfindingState.Type == PathfindingType.Patrol;
        public bool IsMovingWithPathfinding => PathfindingState.Type != PathfindingType.None;
        public bool IsNavToPosition => PathfindingState.Type == PathfindingType.NavToPosition;
        public bool IsNavToObject => PathfindingState.Type == PathfindingType.NavToObject;
        public bool IsPathfindingCombat => PathfindingState.Status == PathfindingStatus.Combat;
        public bool IsPathfindingResetting => PathfindingState.Status == PathfindingStatus.Reset;
        public bool IsNavigating => PathfindingState.Status == PathfindingStatus.Navigating;
        public bool IsIdle => PathfindingState.Status == PathfindingStatus.Idle;
        public bool IsStuck => PathfindingState.StuckCount > 0;

        private HashSet<uint> PathfindingAllies = new HashSet<uint>();

        private bool OriginalAttackable = false;

        /// <summary>
        /// Tick called from Player_Tick and Monster_Tick for pathfinding processing (called every second)
        /// </summary>
        /// <param name="currentUnixTime"></param>
        public void Pathfinding_Tick(double currentUnixTime)
        {
            NextMonsterTickTime = currentUnixTime + monsterTickInterval;

            if (IsMoving)
            {
                PhysicsObj.update_object();

                UpdatePosition_SyncLocation();

                SendUpdatePosition();
            }

            if (!PathfindingState.IsProcessingTick && PathfindingState.NextTickTime < currentUnixTime)
            {
                PathfindingState.IsProcessingTick = true;
                BeginPathfinding(currentUnixTime);
                PathfindingState.IsProcessingTick = false;
            }
        }
        
        /// <summary>
        /// A pathfinding action that navigates the creature to a target position 
        /// </summary>
        /// <param name="position"></param>
        /// <param name="hostileTargetDetectRange"></param>
        public void NavToPosition(Position position, float hostileTargetDetectRange = 20.0f)
        {
            WakeUp(false);
            SetPathfindingAttackable();
            PathfindingState.TargetPosition = position;
            PathfindingState.TargetHostileRange = hostileTargetDetectRange; 
            PathfindingState.Type = PathfindingType.NavToPosition;
            PathfindingState.Status = PathfindingStatus.Navigating;
        }

        /// <summary>
        /// A pathfinding action that navigates the creature to a target WorldObject
        /// </summary>
        /// <param name="wo"></param>
        /// <param name="hostileTargetDetectRange"></param>
        public void NavToObject(WorldObject wo, float hostileTargetDetectRange = 20.0f)
        {
            WakeUp(false);
            SetPathfindingAttackable();
            PathfindingState.TargetObject = wo;
            PathfindingState.TargetHostileRange = hostileTargetDetectRange; 
            PathfindingState.Type = PathfindingType.NavToObject;
            PathfindingState.Status = PathfindingStatus.Navigating;
            wo.AddPathfindingFollower(this);
        }

        /// <summary>
        /// A pathfinding action that navigates the creatures to random points within a landblock. New
        /// paths are created after completing old paths
        /// </summary>
        /// <param name="hostileTargetDetectRange"></param>
        /// <param name="maxDistance"></param>
        public void Patrol(float hostileTargetDetectRange = 20.0f, float? maxDistance = null)
        {
            //log.Info($"Patrolling Triggered");
            WakeUp(false);
            SetPathfindingAttackable();
            PathfindingState.TargetPosition = PathfinderManager.GetRandomPointOnMesh(Location, maxDistance);
            PathfindingState.TargetHostileRange = hostileTargetDetectRange; 
            PathfindingState.Type = PathfindingType.Patrol;
            PathfindingState.Status = PathfindingStatus.Navigating;
            //log.Info($"Starting main position: {PathfindingState.TargetPosition.ToLOCString()}");
            //log.Info($"Starting main target distance: {PathfindingState.TargetPosition.SquaredDistanceTo(Location)}");
        }

        /// <summary>
        /// Make a pathfinding creature attackable, even if it originally wasn't attackable
        /// </summary>
        private void SetPathfindingAttackable()
        {
            OriginalAttackable = Attackable;
            Attackable = true;
        }

        /// <summary>
        /// The main state handler for processing pathfinding every tick
        /// </summary>
        /// <param name="currentUnixTime"></param>
        private void BeginPathfinding(double currentUnixTime)
        {
            PathfindingState.NextTickTime  = currentUnixTime + PathfindingState.MoveTime;

            if (IsIdle)
                return;

            if (IsNavToPosition && PathfindingState.TargetPosition == null)
            {
                log.Info("Target position not provided for nav to position, finishing path finding");
                return;
            }

            if (IsNavToObject && PathfindingState.TargetObject == null)
            {
                log.Info("Target object not provided for nav to object, finishing path finding");
                return;
            }


            if (IsPathfindingCombat)
            {
                //log.Info($"Has initiated an attack: {PathfindingState.HasInitiatedAttack}");
                //log.Info($"Time since last attack: {currentUnixTime - PathfindingState.LastMonsterAttack }");
                return;
            }

            var targetPosition =
                PathfindingState.TemporaryTargetPosition ??
                PathfindingState.HostilePosition ??
                PathfindingState.TargetObject?.PhysicsObj.Position.ACEPosition() ??
                PathfindingState.TargetPosition;

            if (targetPosition == null)
            {
                //log.Info("Couldn't find a target position, finishing path finding");
                FinishPathfinding(true);
                return;
            }

            var distance = PhysicsObj.Position.ACEPosition().SquaredDistanceTo(targetPosition);

            if (distance < 2)
            {
                //log.Info("Reached destination, finishing PathFind");
                FinishPathfinding();
                return;
            }

            if (targetPosition.Landblock != Location.Landblock)
            {
                //log.Info("Objects can only follow other objects within the same landblock");
                FinishPathfinding();
                return;
            }

            if (IsMoving && currentUnixTime < PathfindingState.LastMoveTime + 5)
                return;

            //log.Info($"Distance to path: {distance}");
            var paths = PathfinderManager.FindRoute(PhysicsObj.Position.ACEPosition(), targetPosition);

            if (paths is null || paths.Count == 0)
            {
                //log.Info("Path is null, stopping");
                CancelMoveTo();
                return;
            }

            //log.Info($"PathsCount: {paths.Count}");
            NavToPath(currentUnixTime, paths);
        }

        /// <summary>
        /// Perform the actual server side movement based on the paths and time provided
        /// </summary>
        /// <param name="currentUnixTime"></param>
        /// <param name="paths"></param>
        private void NavToPath(double currentUnixTime, List<Position> paths)
        {
            var destination = paths.Count > 1 ? new Position(paths[1]) : paths[0];

            if (destination == null)
            {
                //log.Info("No paths found! finishing pathfind navigation");
                FinishPathfinding();
                return;
            }

            var lastDistance = PathfindingState.LastPosition?.SquaredDistanceTo(PhysicsObj.Position.ACEPosition());

            // if stuck
            if ((currentUnixTime > PathfindingState.NextStuckBackoff) && (PathfindingState.LastPosition != null) &&
                (lastDistance.HasValue && lastDistance < 0.2))
            {
                IsMoving = true;
                PathfindingState.StuckCount++;

                log.Info("Stuck in position!");
                log.Info($"CurrentLocation: {PhysicsObj.Position.ACEPosition().ToLOCString()}");
                log.Info($"StuckCount: {PathfindingState.StuckCount.ToString()}");

                if (PathfindingState.StuckCount > 1)
                {
                    PathfindingState.NextStuckBackoff = currentUnixTime + 2;
                    ResetPath();
                }

                if (PathfindScan())
                    return;

                var offsetPosition = new ACE.Entity.Position(Location);
                offsetPosition = offsetPosition.InFrontOf(3);
                RunToPosition(offsetPosition);
            } else
            // if not stuck
            {
                PathfindingState.LastMoveTime = currentUnixTime;

                IsMoving = true;
                PathfindingState.StuckCount = 0;

                log.Info($"Navigating to destination: {destination.ToLOCString()}");
                log.Info($"Main target destination: {PathfindingState.TargetPosition.ToLOCString()}");
                if(PathfindScan())
                    return;

                PathfindingState.LastPosition = PhysicsObj.Position.ACEPosition();
                RunToPosition(destination);
            }
        }
    
        /// <summary>
        /// Switch to a temporary path. This allows alternate navigation
        /// when getting stuck
        /// </summary>
        private void ResetPath()
        {
            log.Info("Resetting Path!");

            // ugly solution to find new position within the creature's radius
            float distance = 100;

            while (distance > 75)
            {
                var newPosition = PathfinderManager.GetRandomPointOnMesh(Location);
                PathfindingState.TemporaryTargetPosition = newPosition;
                distance = newPosition != null ? Location.SquaredDistanceTo(newPosition) : distance;
            }

            log.Info($"Reset path distance: {Location.SquaredDistanceTo(PathfindingState.TemporaryTargetPosition)}");
            PathfindingState.LastPosition = null;
            PathfindingState.Status = PathfindingStatus.Reset;
        }

        /// <summary>
        /// Called when pathfinding has been canceled or has reached the end condition
        /// </summary>
        public void FinishPathfinding(bool force = false)
        {
            Attackable = OriginalAttackable;

            if (force)
            {
                HandleForceFinish();
                return;
            }

            if (IsPathfindingResetting)
            {
                log.Info("Finished Reset Path");
                log.Info($"Finished reset path position: {PathfindingState.TemporaryTargetPosition.ToLOCString()}");
                log.Info($"Finished reset path distance: {PathfindingState.TemporaryTargetPosition.SquaredDistanceTo(Location)}");
                PathfindingState.TemporaryTargetPosition = null;
                PathfindingState.Status = PathfindingStatus.Navigating;
                return;
            }

            if (PathfindingState.TemporaryTargetPosition != null)
            {
                log.Info("Finished temporary Path");
                log.Info($"Finished temporary path position: {PathfindingState.TemporaryTargetPosition.ToLOCString()}");
                log.Info($"Finished temporary path distance: {PathfindingState.TemporaryTargetPosition.SquaredDistanceTo(Location)}");
                PathfindingState.TemporaryTargetPosition = null;
                PathfindingState.Status = PathfindingStatus.Navigating;
                return;
            }

            if (PathfindingState.HostilePosition != null)
            {
                log.Info($"Finished hostile target position: {PathfindingState.TargetPosition?.ToLOCString()}");
                log.Info($"Finished hotile target distance: {PathfindingState?.TargetPosition?.SquaredDistanceTo(Location)}");
                PathfindingState.HostilePosition = null;
                PathfindingState.Status = PathfindingStatus.Navigating;
                return;
            }

            if (PathfindingState.TargetObject != null)
            {
                log.Info($"Finished main target object: {PathfindingState.TargetObject?.Location.ToLOCString()}");
                log.Info($"Finished main target object distance: {PathfindingState?.TargetObject?.Location.SquaredDistanceTo(Location)}");
                PathfindingState.TargetObject.RemovePathfindingFollower(this);
                PathfindingState.TargetObject = null;
                PathfindingState.Status = PathfindingStatus.Idle;
                PathfindingState.Type = PathfindingType.None;
                return;
            }

            if (PathfindingState.TargetPosition != null)
            {
                log.Info($"Finished main target position: {PathfindingState.TargetPosition?.ToLOCString()}");
                log.Info($"Finished main target distance: {PathfindingState?.TargetPosition?.SquaredDistanceTo(Location)}");
                PathfindingState.TargetPosition = null;
                PathfindingState.Status = PathfindingStatus.Idle;

                if (IsPatrolCreature)
                    Patrol(PathfindingState.TargetHostileRange);
                else
                    PathfindingState.Type = PathfindingType.None;

                return;
            }
        }

        private void HandleForceFinish()
        {
            if (PathfindingState.TargetObject != null)
            {
                PathfindingState.TargetObject.RemovePathfindingFollower(this);
                PathfindingState.TargetObject = null;
            }

            PathfindingState.TemporaryTargetPosition = null;
            PathfindingState.HostilePosition = null;
            PathfindingState.TargetPosition = null;
            PathfindingState.Type = PathfindingType.None;
            PathfindingState.Status = PathfindingStatus.Idle;
        }

        /// <summary>
        /// 
        /// </summary>
        public void FinishPathfindingCombat()
        {
            AttackTarget = null;
            PathfindingState.Status = PathfindingStatus.Navigating;
            PathfindingState.HasInitiatedAttack = false;

            var hostileLocation = PathfindingState.HostilePosition;

            if (hostileLocation != null && Location.SquaredDistanceTo(hostileLocation) < 50)
            {
                log.Info($"Removing Hostile Position: {PathfindingState.HostilePosition?.ToLOCString()}");
                PathfindingState.HostilePosition = null;
            }

            log.Info("Exit combat state");
        }

        public void EnterPathfindingCombat(Creature creature)
        {
            AttackTarget = creature;
            PathfindingState.LastPosition = null;
            PathfindingState.Status = PathfindingStatus.Combat;
            log.Info("Entered Combat State");
        }

        /// <summary>
        /// Find the closest door within visible range and open it
        /// </summary>
        /// <param name="visibleObjects"></param>
        /// <returns></returns>
        private bool ScanForDoor(List<WorldObject>? visibleObjects = null)
        {
            visibleObjects = visibleObjects ?? PhysicsObj.ObjMaint.GetVisibleObjectsValues()
                .Select(o => o.WeenieObj.WorldObject).Where(wo => wo != null).OrderBy(wo => wo.Location.SquaredDistanceTo(Location)).ToList();
            var closestDoors = visibleObjects.OfType<Door>().ToList();
            var closestDoor = closestDoors.FirstOrDefault();

            var closestDoorDistance = closestDoor?.Location.SquaredDistanceTo(Location);

            if (closestDoor != null && !closestDoor.IsOpen && !closestDoor.IsLocked && closestDoorDistance.HasValue && closestDoorDistance.Value < 50)
            {
                closestDoor.Open();
                return true;
            }

            return false;
        }

        /// <summary>
        /// Check if target creature is an ally 
        /// </summary>
        /// <param name="creature"></param>
        /// <returns></returns>
        public bool HasPathfindingAlly(Creature creature)
        {
            return creature != null && PathfindingAllies.Contains(creature.Guid.Full);
        }

        /// <summary>
        /// Find creatures within visible range and aggro them if close enough
        /// </summary>
        /// <param name="visibleObjects"></param>
        /// <returns></returns>
        private bool ScanForCreatures(List<WorldObject>? visibleObjects = null)
        {
            var visibleTargets = PhysicsObj.ObjMaint.GetVisibleTargetsValuesOfTypeCreature();
            visibleObjects = visibleObjects ?? PhysicsObj.ObjMaint.GetVisibleObjectsValues()
                .Select(o => o.WeenieObj.WorldObject).Where(wo => wo != null).OrderBy(wo => wo.Location.SquaredDistanceTo(Location)).ToList();
            var closestCreatures = visibleObjects.OfType<Creature>().Where(creature =>
                !creature.IsDead &&
                !creature.IsDestroyed &&
                creature is not Admin &&
                !HasPathfindingAlly(creature)).ToList();

            //log.Info($"Scanning creatures count: {closestCreatures.Count}");

            CheckMonsters(closestCreatures);

            var closestCreature = closestCreatures.FirstOrDefault();
            var closestCreatureDistance = closestCreature?.Location.SquaredDistanceTo(Location);

            //log.Info($"Closest monster 2d: {closestCreature?.Location.SquaredDistanceTo(Location)}");
            //log.Info($"Closest monster 3d: {closestCreatureDistance}");

            if (closestCreature != null && closestCreatureDistance.HasValue && closestCreatureDistance.Value < 5)
            {
                EnterPathfindingCombat(closestCreature);
                return true;
            }

            if (PathfindingState.HostilePosition == null && closestCreature != null && closestCreatureDistance.HasValue && closestCreatureDistance.Value < PathfindingState.TargetHostileRange)
            {
                PathfindingState.HostilePosition = closestCreature.PhysicsObj.Position.ACEPosition();
                log.Info($"Adding Hostile Position: {PathfindingState.HostilePosition?.ToLOCString()}");
                return false;
            }

            return false;
        }

        /// <summary>
        /// Main handler for pathfind scanning
        /// </summary>
        /// <returns></returns>
        private bool PathfindScan()
        {
            //NextPathfindScan = currentUnixTime + PathfindScanTime;
            var visibleObjects = PhysicsObj.ObjMaint.GetVisibleObjectsValues()
                .Select(o => o.WeenieObj.WorldObject).Where(wo => wo != null).OrderBy(wo => wo.Location.SquaredDistanceTo(Location)).ToList();

            if (ScanForCreatures(visibleObjects))
                return true;

            if (ScanForDoor(visibleObjects))
                return true;

            return false;
        }

        /// <summary>
        /// When scanning creatures, alert monsters nearby
        /// </summary>
        /// <param name="visibleObjects"></param>
        public virtual void CheckMonsters(List<Creature> visibleObjects)
        {
            foreach (var monster in visibleObjects)
            {
                if (monster is Player) continue;

                var distSq = PhysicsObj.get_distance_sq_to_object(monster.PhysicsObj, true);
                //var distSq = Location.SquaredDistanceTo(monster.Location);

                if (distSq <= monster.VisualAwarenessRangeSq)
                    AlertMonster(monster);
            }
        }

        /// <summary>
        /// A handler when attacking a creature during pathfinding
        /// </summary>
        /// <param name="creature"></param>
        public void OnPathfindCreatureAttack(Creature creature)
        {
            PathfindingState.HasInitiatedAttack = true;
            PathfindingState.LastMonsterAttack = Time.GetUnixTime();
        }

        /// <summary>
        /// Server action that broadcasts and performs server movement with physics
        /// </summary>
        /// <param name="position"></param>
        public void RunToPosition(Position position)
        {
            var moveToPosition = new Motion(this, position);
            moveToPosition.MoveToParameters.DistanceToObject = 0.0f;
            SetWalkRunThreshold(moveToPosition, position);
            EnqueueBroadcastMotion(moveToPosition);
            // perform movement on server
            var mvp = new MovementParameters(); 
            mvp.DistanceToObject = moveToPosition.MoveToParameters.DistanceToObject; 
            //mvp.UseFinalHeading = true;

            PhysicsObj.MoveToPosition(new Physics.Common.Position(position), mvp);

            // prevent snap forward
            PhysicsObj.UpdateTime = Physics.Common.PhysicsTimer.CurrentTime;
        }
    }
};
