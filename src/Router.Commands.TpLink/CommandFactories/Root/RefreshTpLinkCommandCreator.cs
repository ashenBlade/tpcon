using Router.Commands.TpLink.Commands;
using Router.Commands.TpLink.Routers;

namespace Router.Commands.TpLink.CommandFactories;

internal class RefreshTpLinkCommandCreator : SingleTpLinkCommandCreator
{
    public RefreshTpLinkCommandCreator() : base("refresh") { }
    public override IRouterCommand CreateRouterCommand(RouterCommandContext context)
    {
        return new TpLinkRefreshRouterCommand(context.Router);
    }
}