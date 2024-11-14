using System.Collections.Generic;
using ACE.Server.DotRecast.Core.Numerics;

namespace ACE.Server.DotRecast.Detour.Extras.Jumplink
{
    public class EdgeSampler
    {
        public readonly GroundSegment start = new GroundSegment();
        public readonly List<GroundSegment> end = new List<GroundSegment>();
        public readonly Trajectory trajectory;

        public readonly RcVec3f ax = new RcVec3f();
        public readonly RcVec3f ay = new RcVec3f();
        public readonly RcVec3f az = new RcVec3f();

        public EdgeSampler(JumpEdge edge, Trajectory trajectory)
        {
            this.trajectory = trajectory;
            ax = RcVec3f.Subtract(edge.sq, edge.sp);
            ax = RcVec3f.Normalize(ax);
            
            az = new RcVec3f(ax.Z, 0, -ax.X);
            az = RcVec3f.Normalize(az);
            
            ay = new RcVec3f(0, 1, 0);
        }
    }
}