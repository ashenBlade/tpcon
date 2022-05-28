namespace Router.TpLink.CommandCreator.Lan;

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