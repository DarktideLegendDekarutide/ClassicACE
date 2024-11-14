using ACE.Common;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Server.Entity;
using ACE.Server.Entity.Actions;
using ACE.Server.Managers;
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
            NavToObject,
            NavToPosition,
            Patrol
        }

        private PathfindingType _pathFindingType = PathfindingType.NavToPosition;

        private Position? PathfindingTargetPosition;
        private WorldObject? PathfindingTargetObject;

        private Position? LastPathfindingPosition;
        public bool IsMovingWithPathfinding { get; private set; } = false;

        private double NextPathfindingMoveTime = 0;
        private double LastPathfindingMoveTime = 0;
        private readonly double PathfindingMoveTime = 1;

        public void RunToPosition(Position position)
        {
            var moveToPosition = new Motion(this, position);
            moveToPosition.RunRate = GetRunRate();
            EnqueueBroadcastMotion(moveToPosition);
        }

        internal void NavToPosition(Position position)
        {
            PathfindingTargetPosition = position;
            IsMovingWithPathfinding = true;
            _pathFindingType = PathfindingType.NavToPosition;
        }

        internal void NavToObject(WorldObject wo)
        {
            PathfindingTargetObject = wo;
            IsMovingWithPathfinding = true;
            _pathFindingType = PathfindingType.NavToObject;
        }

        private void BeginPathfind(double currentUnixTime)
        {
            if (_pathFindingType == PathfindingType.NavToPosition && PathfindingTargetPosition == null)
                return;

            if (_pathFindingType == PathfindingType.NavToObject && PathfindingTargetObject == null)
                return;

            NextPathfindingMoveTime = currentUnixTime + PathfindingMoveTime;

            var targetPosition = PathfindingTargetObject?.PhysicsObj.Position.ACEPosition() ?? PathfindingTargetPosition;

            if (targetPosition.LandblockId != Location.LandblockId)
            {
                log.Info("Objects can only follow other objects within the same landblock");
                FinishPathfinding();
                return;
            }

            var isStuck = (currentUnixTime - LastPathfindingMoveTime) > 5.0;

            if (IsMoving && !isStuck)
            {
                return;
            }

            var distance = PhysicsObj.Position.ACEPosition().DistanceTo(targetPosition);
            log.Info($"Distance: {distance}");

            if (distance < 2)
            {
                log.Info("Distance has been reached");
                FinishPathfinding();
                return;
            }

            var path = PathfinderManager.FindRoute(PhysicsObj.Position.ACEPosition(), targetPosition);

            if (path is null)
            {
                log.Info("Path is null, stopping");
                CancelMoveTo();
                return;
            }

            log.Info($"PathCount: {path.Count}");

            if (path.Count < 2)
            {
                log.Info("Path is less than 3, recalculating path");
                LastPathfindingPosition = null;
                CancelMoveTo();
            } else
            {
                var destination = new Position(path.Skip(1).FirstOrDefault());

                LastPathfindingMoveTime = currentUnixTime;

                if (LastPathfindingPosition != null && LastPathfindingPosition.DistanceTo(PhysicsObj.Position.ACEPosition()) < 1)
                {
                    log.Info("Stuck in position, resetting");
                    log.Info($"CurrentLocation: {PhysicsObj.Position.ACEPosition().ToLOCString()}");

                    CancelMoveTo();
                    IsMoving = true;


                    var offsetPosition = new ACE.Entity.Position(Location);

                    /*// Generate a random angle between 0 and 2π radians for a full 360-degree rotation
                    float randomAngleRadians = ((float)ThreadSafeRandom.Next(0.0f, 360.0f)).ToRadians();

                    // Create a quaternion representing the random rotation
                    Quaternion randomRotation = Quaternion.CreateFromYawPitchRoll(randomAngleRadians, 0, 0);

                    // Apply the random rotation to the offset position
                    offsetPosition.Rotation = new Quaternion(0, 0, offsetPosition.RotationZ, offsetPosition.RotationW) * randomRotation;*/

                    // Move the position forward by 3 units
                    offsetPosition = offsetPosition.InFrontOf(3);

                    // Update the last pathfinding position
                    LastPathfindingPosition = PhysicsObj.Position.ACEPosition();

                    // Move to the new position with physics
                    MoveToPositionWithPhysics(offsetPosition);
                } else
                {
                    IsMoving = true;
                    log.Info($"CurrentLocation: {PhysicsObj.Position.ACEPosition().ToLOCString()}");
                    log.Info($"NextLocation: {destination.ToLOCString()}");
                    LastPathfindingPosition = PhysicsObj.Position.ACEPosition();
                    MoveToPositionWithPhysics(destination);
                }
            }
        }

        public void FinishPathfinding()
        {
            PathfindingTargetPosition = null;
            PathfindingTargetObject = null;
            IsMovingWithPathfinding = false;
        }

        public void Pathfinding_Tick(double currentUnixTime)
        {
            NextMonsterTickTime = currentUnixTime + monsterTickInterval;

            PhysicsObj.update_object();

            UpdatePosition_SyncLocation();

            SendUpdatePosition();

            //HandleFindTarget();

            if (NextPathfindingMoveTime < currentUnixTime)
            {
                BeginPathfind(currentUnixTime);
            }
        }

    }
};
