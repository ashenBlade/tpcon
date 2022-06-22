namespace Router.Commands.TpLink.CommandFactory;

internal class RootTpLinkCommandFactory : CompositeTpLinkCommandFactory
{
    public RootTpLinkCommandFactory(IEnumerable<KeyValuePair<string, Func<BaseTpLinkCommandFactory>>> commands)
        : base(commands)
    {
    }
}