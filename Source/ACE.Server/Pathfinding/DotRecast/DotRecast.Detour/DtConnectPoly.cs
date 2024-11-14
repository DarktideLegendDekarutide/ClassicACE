using System.Runtime.InteropServices;

namespace ACE.Server.DotRecast.Detour
{
    public readonly struct DtConnectPoly
    {
        public readonly long refs;
        public readonly float tmin;
        public readonly float tmax;

        public DtConnectPoly(long refs, float tmin, float tmax)
        {
            this.refs = refs;
            this.tmin = tmin;
            this.tmax = tmax;
        }
    }
}