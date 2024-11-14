﻿using ACE.Server.DotRecast.Core.Numerics;

namespace ACE.Server.DotRecast.Recast.Geom
{
    public class RcChunkyTriMeshNode
    {
        public RcVec2f bmin;
        public RcVec2f bmax;
        public int i;
        public int[] tris;
    }
}