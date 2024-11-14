using ACE.Server.DotRecast.Detour.Dynamic.Colliders;
using ACE.Server.DotRecast.Recast.Toolset.Gizmos;

namespace ACE.Server.DotRecast.Recast.Toolset.Gizmos
{
    public class RcGizmo
    {
        public readonly IRcGizmoMeshFilter Gizmo;
        public readonly IDtCollider Collider;

        public RcGizmo(IDtCollider collider, IRcGizmoMeshFilter gizmo)
        {
            Collider = collider;
            Gizmo = gizmo;
        }
    }
}