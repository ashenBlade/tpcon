using Router.TpLink.CommandCreators;

namespace Router.TpLink.TLWR741ND.CommandCreators.Lan;

internal class LanCompositeCommandCreator : CompositeTpLinkCommandCreator
{
    private static IEnumerable<TpLinkCommandCreator> LanCommands
        => new TpLinkCommandCreator[]
           {
               new GetLanStatusCommandCreator()
           };
    public LanCompositeCommandCreator()
        : base(LanCommands, "lan")
    { }
}