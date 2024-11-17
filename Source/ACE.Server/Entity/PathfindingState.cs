using ACE.Entity;
using ACE.Server.WorldObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public double MoveTime { get; } = 1;
        public double NextScan { get; set; } = 0;
        public double ScanTime { get; } = 10;
        public double NextStuckBackoff = 0;
        public uint StuckCount = 0;
        public bool IsProcessingTick { get; set; } = false;
        public Position? LastPosition { get; set; } = null;
        public Position? TemporaryTargetPosition { get; set; } = null;
        public Position? TargetPosition { get; set; } = null;
        public WorldObject? TargetObject { get; set; } = null;
        public PathfindingState() { }
    }

}
