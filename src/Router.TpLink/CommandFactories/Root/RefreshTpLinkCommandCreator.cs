using Router.Commands;
using Router.TpLink.Commands;

namespace Router.TpLink.CommandFactories.Root;

internal class RefreshTpLinkCommandCreator : SingleTpLinkCommandCreator
{
    public RefreshTpLinkCommandCreator() : base("refresh") { }
    public override IRouterCommand CreateRouterCommand(RouterCommandContext context)
    {
        return new TpLinkRefreshRouterCommand(context.Router);
    }
}