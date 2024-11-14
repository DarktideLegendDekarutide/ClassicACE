﻿using ACE.Server.DotRecast.Core.Numerics;
using static ACE.Server.DotRecast.Recast.Toolset.Gizmos.RcGizmoHelper;


namespace ACE.Server.DotRecast.Recast.Toolset.Gizmos
{
    public class RcSphereGizmo : IRcGizmoMeshFilter
    {
        public readonly float[] vertices;
        public readonly int[] triangles;
        public readonly float radius;
        public readonly RcVec3f center;

        public RcSphereGizmo(RcVec3f center, float radius)
        {
            this.center = center;
            this.radius = radius;
            vertices = GenerateSphericalVertices();
            triangles = GenerateSphericalTriangles();
        }
    }
}