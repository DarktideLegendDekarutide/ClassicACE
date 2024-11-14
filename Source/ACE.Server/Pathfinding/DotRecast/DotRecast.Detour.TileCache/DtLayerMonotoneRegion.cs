using System.Collections.Generic;

namespace ACE.Server.DotRecast.Detour.TileCache
{
    public class DtLayerMonotoneRegion
    {
        public const int DT_LAYER_MAX_NEIS = 16;

        public int area;
        public List<int> neis = new List<int>(DT_LAYER_MAX_NEIS);
        public int regId;
        public int areaId;
    };
}