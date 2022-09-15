namespace Router.Commands.TpLink.CommandFactory;

internal class RootTpLinkCommandFactory : CompositeTpLinkCommandFactory
{
    public RootTpLinkCommandFactory(
        IEnumerable<KeyValuePair<CommandFactoryInfo, Func<BaseTpLinkCommandFactory>>> commands)
        : base(commands)
    {
    }
}