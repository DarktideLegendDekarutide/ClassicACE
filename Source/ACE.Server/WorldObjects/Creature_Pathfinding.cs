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

        public bool IsPatrolCreature => PathfindingState.Type == PathfindingState.PathfindingType.Patrol;
        public bool IsMovingWithPathfinding => PathfindingState.Type != PathfindingState.PathfindingType.None;
        public bool IsNavToPosition => PathfindingState.Type == PathfindingState.PathfindingType.NavToPosition;
        public bool IsNavToObject => PathfindingState.Type == PathfindingState.PathfindingType.NavToObject;
        public bool IsPathfindingCombat => PathfindingState.Status == PathfindingState.PathfindingStatus.Combat;
        public bool IsPathfindingResetting => PathfindingState.Status == PathfindingState.PathfindingStatus.Reset;

        public void Pathfinding_Tick(double currentUnixTime)
        {
            NextMonsterTickTime = currentUnixTime + monsterTickInterval;

            PhysicsObj.update_object();

            UpdatePosition_SyncLocation();

            SendUpdatePosition();

            if (!PathfindingState.IsProcessingTick && PathfindingState.NextTickTime < currentUnixTime)
            {
                PathfindingState.IsProcessingTick = true;
                BeginPathfinding(currentUnixTime);
                PathfindingState.IsProcessingTick = false;
            }
        }


        public void NavToPosition(Position position, float hostileTargetDetectRange = 20.0f)
        {
            WakeUp(false);
            PathfindingState.TargetPosition = position;
            PathfindingState.TargetHostileRange = hostileTargetDetectRange; 
            PathfindingState.Type = PathfindingState.PathfindingType.NavToPosition;
        }

        public void NavToObject(WorldObject wo, float hostileTargetDetectRange = 20.0f)
        {
            WakeUp(false);
            PathfindingState.TargetObject = wo;
            PathfindingState.TargetHostileRange = hostileTargetDetectRange; 
            PathfindingState.Type = PathfindingState.PathfindingType.NavToObject;
        }

        public void Patrol(float hostileTargetDetectRange = 20.0f, float? maxDistance = null)
        {
            WakeUp(false);
            PathfindingState.TargetPosition = PathfinderManager.GetRandomPointOnMesh(Location, maxDistance);
            PathfindingState.TargetHostileRange = hostileTargetDetectRange; 
            PathfindingState.Type = PathfindingState.PathfindingType.Patrol;
        }


        private void BeginPathfinding(double currentUnixTime)
        {
            PathfindingState.NextTickTime  = currentUnixTime + PathfindingState.MoveTime;

            if (IsNavToPosition && PathfindingState.TargetPosition == null)
                return;

            if (IsNavToObject && PathfindingState.TargetObject == null)
                return;

            if (IsPathfindingCombat)
                return;

            var targetPosition = PathfindingState.TargetObject?.PhysicsObj.Position.ACEPosition() ?? PathfindingState.TargetPosition;

            if (IsPathfindingResetting)
                targetPosition = PathfindingState.TargetPosition;

            if (targetPosition == null)
            {
                //log.Info("Couldn't find a target position, finishing path finding");
                FinishPathfinding();
                return;
            }

            var distance = PhysicsObj.Position.ACEPosition().Distance2DSquared(targetPosition);

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

            if (paths is null)
            {
                //log.Info("Path is null, stopping");
                CancelMoveTo();
                return;
            }

            //log.Info($"PathsCount: {paths.Count}");
            NavToPath(currentUnixTime, paths);
        }



        private void NavToPath(double currentUnixTime, List<Position> paths)
        {
            var destination = paths.Count > 1 ? new Position(paths[1]) : paths[0];

            if (destination == null)
            {
                //log.Info("No paths found! finishing pathfind navigation");
                FinishPathfinding();
                return;
            }

            var lastDistance = PathfindingState.LastPosition?.Distance2DSquared(PhysicsObj.Position.ACEPosition());

            // handle being stuck first
            if ((currentUnixTime > PathfindingState.NextStuckBackoff) &&
                (PathfindingState.LastPosition != null) &&
                (lastDistance.HasValue && lastDistance < 0.2))
            {
                IsMoving = true;
                PathfindingState.StuckCount++;

                //log.Info("Stuck in position!");
                //log.Info($"CurrentLocation: {PhysicsObj.Position.ACEPosition().ToLOCString()}");
                //log.Info($"StuckCount: {PathfindingState.StuckCount.ToString()}");

                if (PathfindingState.StuckCount > 1)
                    ResetPath();

                if (PathfindScan())
                    return;

                var offsetPosition = new ACE.Entity.Position(Location);
                offsetPosition = offsetPosition.InFrontOf(3);
                RunToPosition(offsetPosition);
            } else
            {
                PathfindingState.LastMoveTime = currentUnixTime;

                IsMoving = true;
                PathfindingState.StuckCount = 0;
                //log.Info($"CurrentLocation: {PhysicsObj.Position.ACEPosition().ToLOCString()}");
                //log.Info($"NextLocation: {destination.ToLOCString()}");
                ScanForDoor();
                PathfindingState.LastPosition = PhysicsObj.Position.ACEPosition();
                RunToPosition(destination);
            }
        }

        private void ResetPath()
        {
            log.Info("Resetting Path!");
            //NextStuckBackoff = currentUnixTime + 10;

            // If first time resetting, assign the current primary position to the temporary position
            if (!IsPathfindingResetting)
                PathfindingState.TemporaryTargetPosition = PathfindingState.TargetPosition;

            PathfindingState.TargetPosition = PathfinderManager.GetRandomPointOnMesh(Location, 100.0f);
            PathfindingState.LastPosition = null;
            PathfindingState.Status = PathfindingState.PathfindingStatus.Reset;
        }

        public void FinishPathfinding()
        {

            PathfindingState.TargetPosition = null;
            PathfindingState.TargetObject = null;

            if (IsPathfindingResetting)
            {
                PathfindingState.TargetPosition = PathfindingState.TemporaryTargetPosition;
                PathfindingState.TemporaryTargetPosition = null;
                PathfindingState.Status = PathfindingState.PathfindingStatus.Idle;
            } else if (IsPatrolCreature)
                Patrol(PathfindingState.TargetHostileRange);
            else
            {
                PathfindingState.Type = PathfindingState.PathfindingType.None;
                PathfindingState.Status = PathfindingState.PathfindingStatus.Idle;
            }
        }
        public void SetPathfindingIdle()
        {
            PathfindingState.Status = PathfindingState.PathfindingStatus.Idle;
        }

        private bool ScanForDoor(List<WorldObject>? visibleObjects = null)
        {
            visibleObjects = visibleObjects ?? PhysicsObj.ObjMaint.GetVisibleObjectsValues()
                .Select(o => o.WeenieObj.WorldObject).Where(wo => wo != null).OrderBy(wo => wo.Location.Distance2DSquared(Location)).ToList();
            var closestDoors = visibleObjects.OfType<Door>().ToList();
            var closestDoor = closestDoors.FirstOrDefault();

            var closestDoorDistance = closestDoor?.Location.Distance2DSquared(Location);

            if (closestDoor != null && !closestDoor.IsOpen && !closestDoor.IsLocked && closestDoorDistance.HasValue && closestDoorDistance.Value < 50)
            {
                closestDoor.Open();
                return true;
            }

            return false;
        }
        private bool ScanForCreatures(List<WorldObject>? visibleObjects = null)
        {
            visibleObjects = visibleObjects ?? PhysicsObj.ObjMaint.GetVisibleObjectsValues()
                .Select(o => o.WeenieObj.WorldObject).Where(wo => wo != null).OrderBy(wo => wo.Location.Distance2DSquared(Location)).ToList();
            var closestCreatures = visibleObjects.OfType<Creature>().Where(creature => !creature.IsDead).ToList();
            var closestCreature = closestCreatures.FirstOrDefault();

            if (closestCreature != null && closestCreature.Location.Distance2DSquared(Location) < 2)
            {
                AttackTarget = closestCreature;
                PathfindingState.LastPosition = null;
                PathfindingState.Status = PathfindingState.PathfindingStatus.Combat;
                return true;
            }

            return false;
        }

        private bool PathfindScan()
        {
            //NextPathfindScan = currentUnixTime + PathfindScanTime;
            var visibleObjects = PhysicsObj.ObjMaint.GetVisibleObjectsValues()
                .Select(o => o.WeenieObj.WorldObject).Where(wo => wo != null).OrderBy(wo => wo.Location.Distance2DSquared(Location)).ToList();

            if (ScanForCreatures(visibleObjects))
                return true;

            if (ScanForDoor(visibleObjects))
                return true;

            return false;
        }

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
