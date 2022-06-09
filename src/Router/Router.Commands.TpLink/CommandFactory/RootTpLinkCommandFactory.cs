namespace Router.Commands.TpLink.CommandFactory;

internal class RootTpLinkCommandFactory : CompositeTpLinkCommandFactory
{
    public RootTpLinkCommandFactory(IEnumerable<TpLinkCommandFactory> commands)
        : base(commands, string.Empty)
    { }
}