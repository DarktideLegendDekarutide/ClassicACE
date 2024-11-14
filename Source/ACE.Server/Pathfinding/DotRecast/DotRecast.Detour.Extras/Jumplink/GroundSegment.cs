using ACE.Server.DotRecast.Core.Numerics;

namespace ACE.Server.DotRecast.Detour.Extras.Jumplink
{
    public class GroundSegment
    {
        public RcVec3f p = new RcVec3f();
        public RcVec3f q = new RcVec3f();
        public GroundSample[] gsamples;
        public float height;
    }
}