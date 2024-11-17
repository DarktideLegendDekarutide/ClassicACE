using ACE.Entity;

using ACE.Server.Pathfinding;
using System.Collections.Generic;

namespace ACE.Server.Managers
{
    public static class PathfinderManager
    {
        private static PathFinder _pathFinder { get; set; }

        public static void Init()
        {
            _pathFinder = new PathFinder();
        }

        public static List<Position> FindRoute(Position start, Position end)
        {
            return _pathFinder.FindRoute(start, end);
        }

        public static Position? GetRandomPointOnMesh(Position start, float? maxDistance = null)
        {
            return _pathFinder.GetRandomPointOnMesh(start, maxDistance);
        }
    }
}

