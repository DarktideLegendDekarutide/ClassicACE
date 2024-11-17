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
        private enum PathfindingType
        {
            None,
            NavToObject,
            NavToPosition,
            Patrol
        }

        private enum PathfindingState
        {
            Idle,
            Combat,
            Reset,
            FinishedReset
        }

        private PathfindingType PfType = PathfindingType.None;
        private PathfindingState PfState = PathfindingState.Idle;

        private Position? TemporaryTargetPosition;
        private Position? PathfindingTargetPosition;
        private WorldObject? PathfindingTargetObject;
        private float PathfindingTargetPvpRange = 20.0f;
        private double NextPathfindingTickTime = 0;
        private Position? LastPathfindingPosition;
        private double LastPathfindingMoveTime = 0;
        private readonly double PathfindingMoveTime = 1;
        private double NextPathfindScan = 0;
        private readonly double PathfindScanTime = 10;
        private bool IsPathfinding = false;

        private List<WorldObject> PathGuides = new List<WorldObject>();

        public bool IsPatrolCreature
        {
            get
            {
                return PfType == PathfindingType.Patrol;
            }
        }

        public bool IsMovingWithPathfinding
        {
            get
            {
                return PfType != PathfindingType.None;
            }
        }

        public void Pathfinding_Tick(double currentUnixTime)
        {
            NextMonsterTickTime = currentUnixTime + monsterTickInterval;

            PhysicsObj.update_object();

            UpdatePosition_SyncLocation();

            SendUpdatePosition();

            if (!IsPathfinding && NextPathfindingTickTime < currentUnixTime)
            {
                IsPathfinding = true;
                BeginPathfinding(currentUnixTime);
                IsPathfinding = false;
            }
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
                LastPathfindingPosition = null;
                PfState = PathfindingState.Combat;
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

        public void NavToPosition(Position position, float hostileTargetDetectRange = 20.0f)
        {
            WakeUp(false);
            PathfindingTargetPosition = position;
            PathfindingTargetPvpRange = hostileTargetDetectRange; 
            PfType = PathfindingType.NavToPosition;
        }

        public void NavToObject(WorldObject wo, float hostileTargetDetectRange = 20.0f)
        {
            WakeUp(false);
            PathfindingTargetObject = wo;
            PathfindingTargetPvpRange = hostileTargetDetectRange; 
            PfType = PathfindingType.NavToObject;
        }

        public void Patrol(float hostileTargetDetectRange = 20.0f, float? maxDistance = null)
        {
            WakeUp(false);
            PathfindingTargetPosition = PathfinderManager.GetRandomPointOnMesh(Location, maxDistance);
            PathfindingTargetPvpRange = hostileTargetDetectRange; 
            PfType = PathfindingType.Patrol;
        }


        private void BeginPathfinding(double currentUnixTime)
        {
            NextPathfindingTickTime = currentUnixTime + PathfindingMoveTime;

            PathGuides.Clear();

            if (PfType == PathfindingType.NavToPosition && PathfindingTargetPosition == null)
                return;

            if (PfType == PathfindingType.NavToObject && PathfindingTargetObject == null)
                return;

            if (PfState == PathfindingState.Combat)
                return;

            var targetPosition = PathfindingTargetObject?.PhysicsObj.Position.ACEPosition() ?? PathfindingTargetPosition;

            if (PfState == PathfindingState.Reset)
                targetPosition = PathfindingTargetPosition;

            if (PfState == PathfindingState.FinishedReset)
            {
                PathfindingTargetPosition = TemporaryTargetPosition;
                TemporaryTargetPosition = null;
            }

            if (targetPosition == null)
            {
                log.Info("Couldn't find a target position, finishing path finding");
                FinishPathfinding();
                return;
            }

            var distance = PhysicsObj.Position.ACEPosition().Distance2DSquared(targetPosition);

            if (distance < 2)
            {
                log.Info("Reached destination, finishing PathFind");
                FinishPathfinding();
                return;
            }

            if (targetPosition.Landblock != Location.Landblock)
            {
                log.Info("Objects can only follow other objects within the same landblock");
                FinishPathfinding();
                return;
            }

            if (IsMoving && currentUnixTime < LastPathfindingMoveTime + 5)
                return;

            log.Info($"Distance to path: {distance}");
            var paths = PathfinderManager.FindRoute(PhysicsObj.Position.ACEPosition(), targetPosition);

            if (paths is null)
            {
                log.Info("Path is null, stopping");
                CancelMoveTo();
                return;
            }

            log.Info($"PathsCount: {paths.Count}");
            NavToPath(currentUnixTime, paths);
        }

        private double NextStuckBackoff = 0;
        private uint StuckCount = 0;

        private void NavToPath(double currentUnixTime, List<Position> path)
        {
            var destination = new Position(path.Skip(1).FirstOrDefault());

            var lastDistance = LastPathfindingPosition?.Distance2DSquared(PhysicsObj.Position.ACEPosition());

            if ((currentUnixTime > NextStuckBackoff) &&
                (LastPathfindingPosition != null) &&
                (lastDistance.HasValue && lastDistance < 0.2))
            {
                IsMoving = true;
                StuckCount++;

                log.Info("Stuck in position, resetting");
                log.Info($"CurrentLocation: {PhysicsObj.Position.ACEPosition().ToLOCString()}");
                log.Info($"StuckCount: {StuckCount.ToString()}");

                if (StuckCount == 5)
                {
                    PfState = PathfindingState.Reset;
                    TemporaryTargetPosition = PathfindingTargetPosition;
                    PathfindingTargetPosition = Location.InFrontOf(-3);
                    NextStuckBackoff = currentUnixTime + 10;
                    LastPathfindingPosition = null;
                }

                if (PathfindScan())
                    return;

                var offsetPosition = new ACE.Entity.Position(Location);
                offsetPosition = offsetPosition.InFrontOf(3);
                RunToPosition(offsetPosition);
            } else
            {
                LastPathfindingMoveTime = currentUnixTime;

                IsMoving = true;
                StuckCount = 0;
                log.Info($"CurrentLocation: {PhysicsObj.Position.ACEPosition().ToLOCString()}");
                log.Info($"NextLocation: {destination.ToLOCString()}");
                ScanForDoor();
                LastPathfindingPosition = PhysicsObj.Position.ACEPosition();
                RunToPosition(destination);
            }
        }

        public void FinishPathfinding()
        {
            if (TemporaryTargetPosition != null)
                PathfindingTargetPosition = TemporaryTargetPosition;

            PathfindingTargetPosition = null;
            PathfindingTargetObject = null;
            PfState = PathfindingState.Idle;
            if (PfType == PathfindingType.Patrol)
                Patrol(PathfindingTargetPvpRange);
            else 
                PfType = PathfindingType.None;
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
