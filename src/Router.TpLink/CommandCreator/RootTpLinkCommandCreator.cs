namespace Router.TpLink.CommandCreator;

internal class RootTpLinkCommandCreator : CompositeTpLinkCommandCreator
{
    public RootTpLinkCommandCreator(IEnumerable<TpLinkCommandCreator> commands)
        : base(commands, string.Empty)
    { }
}