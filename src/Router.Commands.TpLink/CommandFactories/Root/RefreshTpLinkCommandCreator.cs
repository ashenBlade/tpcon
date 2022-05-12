using Router.Commands.TpLink.Commands;

namespace Router.Commands.TpLink.CommandFactories.Root;

internal class RefreshTpLinkCommandCreator : SingleTpLinkCommandCreator
{
    public RefreshTpLinkCommandCreator() : base("refresh") { }
    public override IRouterCommand CreateRouterCommand(RouterCommandContext context)
    {
        return new TpLinkRefreshRouterCommand(context.Router);
    }
}