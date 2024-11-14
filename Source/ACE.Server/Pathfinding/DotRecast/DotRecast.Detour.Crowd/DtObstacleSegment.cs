﻿using ACE.Server.DotRecast.Core.Numerics;

namespace ACE.Server.DotRecast.Detour.Crowd
{
    public class DtObstacleSegment
    {
        /** End points of the obstacle segment */
        public RcVec3f p = new RcVec3f();

        /** End points of the obstacle segment */
        public RcVec3f q = new RcVec3f();

        public bool touch;
    }
}