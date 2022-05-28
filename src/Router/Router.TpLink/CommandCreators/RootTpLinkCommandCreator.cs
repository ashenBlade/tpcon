namespace Router.TpLink.CommandCreators;

internal class RootTpLinkCommandCreator : CompositeTpLinkCommandCreator
{
    public RootTpLinkCommandCreator(IEnumerable<TpLinkCommandCreator> commands)
        : base(commands, string.Empty)
    { }
}