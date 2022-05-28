using Router.TpLink.CommandCreator;

namespace Router.TpLink.TLWR741ND.CommandCreator.Lan;

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