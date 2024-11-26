using ACE.Entity;
using ACE.Server.WorldObjects;
using System;

namespace ACE.Server.Entity
{
    public enum PathfindingType
    {
        None,
        NavToObject,
        NavToPosition,
        Patrol
    }

    public enum PathfindingStatus
    {
        Idle,
        Navigating,
        Combat,
        Reset
    }

    public class PathfindingState
    {
        public PathfindingType Type { get; set; } = PathfindingType.None;
        public PathfindingStatus Status { get; set; } = PathfindingStatus.Idle;
        public float TargetHostileRange { get; set; } = 20.0f;
        public double NextTickTime { get; set; } = 0;
        public double LastMoveTime { get; set; } = 0;
        public double MoveTime { get; private set;  } = 1;
        public double NextScan { get; set; } = 0;
        public double ScanTime { get; private set; } = 10;
        public double NextStuckBackoff { get; set; } = 0;
        public double LastMonsterAttack { get; set; } = 0;
        public uint StuckCount { get; set; } = 0;
        public bool HasInitiatedAttack { get; set; } = false;
        public bool IsProcessingTick { get;  set; } = false;
        public Position? LastPosition { get; set; } = null;
        public Position? TemporaryTargetPosition { get; set; } = null;
        public Position? TargetPosition { get; set; } = null;
        public Position? HostilePosition { get; set; } = null;
        public WeakReference<WorldObject>? TargetObject;

        public PathfindingState()
        {
            Init();
        }

        public void Init(
            PathfindingType type = PathfindingType.None,
            PathfindingStatus status = PathfindingStatus.Idle,
            float targetHostileRange = 20.0f,
            double scanTime = 10,
            double moveTime = 1)
        {
            Type = type;
            Status = status;
            TargetHostileRange = targetHostileRange;
            NextTickTime = 0;
            LastMoveTime = 0;
            MoveTime = moveTime;
            NextScan = 0;
            ScanTime = scanTime;
            NextStuckBackoff = 0;
            LastMonsterAttack = 0;
            StuckCount = 0;
            HasInitiatedAttack = false;
            IsProcessingTick = false;
            LastPosition = null;
            TemporaryTargetPosition = null;
            TargetPosition = null;
            HostilePosition = null;
            TargetObject = null;
        }

        public void NavToObject(WorldObject wo, float hostileTargetDetectRange = 20.0f)
        {
            if (Status != PathfindingStatus.Idle)
                throw new Exception("A creature can only transition to a pathfinding type from idle status");

            TargetObject = new WeakReference<WorldObject>(wo);
            TargetHostileRange = hostileTargetDetectRange;
            Type = PathfindingType.NavToObject;
            Status = PathfindingStatus.Navigating;
        }

        public void Patrol(Position startingTarget, float hostileTargetDetectRange = 20.0f, float? maxDistance = null)
        {
            if (Status != PathfindingStatus.Idle)
                throw new Exception("A creature can only transition to a pathfinding type from idle status");

            TargetPosition = startingTarget;
            TargetHostileRange = hostileTargetDetectRange; 
            Type = PathfindingType.Patrol;
            Status = PathfindingStatus.Navigating;
        }

        public void NavToPosition(Position position, float hostileTargetDetectRange = 20.0f)
        {
            if (Status != PathfindingStatus.Idle)
                throw new Exception("A creature can only transition to a pathfinding type from idle status");

            TargetPosition = position;
            TargetHostileRange = hostileTargetDetectRange; 
            Type = PathfindingType.NavToPosition;
            Status = PathfindingStatus.Navigating;
        }

        internal void EnterPathFindingCombat()
        {
            LastPosition = null;
            Status = PathfindingStatus.Combat;
        }

        internal void LeavePathfindingCombat(bool clearHostilePosition = false)
        {
            Status = PathfindingStatus.Navigating;
            HasInitiatedAttack = false;

            if (clearHostilePosition)
                HostilePosition = null;
        }

        internal void ResetPath(Position resetPosition)
        {
            LastPosition = null;
            TemporaryTargetPosition = resetPosition;
            Status = PathfindingStatus.Reset;
        }

        internal void EndTemporaryTarget()
        {
            TemporaryTargetPosition = null;
            Status = PathfindingStatus.Navigating;
        }

        internal void EndHostileTarget()
        {
            HostilePosition = null;
            Status = PathfindingStatus.Navigating;
        }
    }
}

