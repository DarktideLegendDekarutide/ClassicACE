using ACE.Server.DotRecast.Core.Numerics;

namespace ACE.Server.DotRecast.Detour
{
    public class DtNoOpDtPolygonByCircleConstraint : IDtPolygonByCircleConstraint
    {
        public static readonly DtNoOpDtPolygonByCircleConstraint Shared = new DtNoOpDtPolygonByCircleConstraint();

        private DtNoOpDtPolygonByCircleConstraint()
        {
        }

        public float[] Apply(float[] polyVerts, RcVec3f circleCenter, float radius)
        {
            return polyVerts;
        }
    }
}