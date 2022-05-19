namespace Router.TpLink.CommandFactories.Lan;

internal class LanCompositeCommandFactory : CompositeTpLinkCommandFactory
{
    private static IEnumerable<InternalTpLinkCommandFactory> LanCommands
        => new InternalTpLinkCommandFactory[]
           {
               new GetLanStatusCommandFactory()
           }; 
    public LanCompositeCommandFactory() 
        : base(LanCommands, "lan")
    { }
}