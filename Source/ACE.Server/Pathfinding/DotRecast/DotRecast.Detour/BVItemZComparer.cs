using System.Collections.Generic;

namespace ACE.Server.DotRecast.Detour
{
    public class BVItemZComparer : IComparer<BVItem>
    {
        public static readonly BVItemZComparer Shared = new BVItemZComparer();

        private BVItemZComparer()
        {
        }

        public int Compare(BVItem a, BVItem b)
        {
            return a.bmin[2].CompareTo(b.bmin[2]);
        }
    }
}