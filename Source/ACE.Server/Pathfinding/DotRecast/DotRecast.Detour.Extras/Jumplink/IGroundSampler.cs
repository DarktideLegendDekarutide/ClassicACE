using ACE.Server.DotRecast.Recast;

namespace ACE.Server.DotRecast.Detour.Extras.Jumplink
{
    public interface IGroundSampler
    {
        void Sample(JumpLinkBuilderConfig acfg, RcBuilderResult result, EdgeSampler es);
    }
}