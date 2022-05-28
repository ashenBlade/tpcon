using Router.Commands;
using Router.TpLink.Commands;

namespace Router.TpLink.CommandCreator.Root;

internal class RefreshTpLinkCommandCreator : SingleTpLinkCommandCreator
{
    public RefreshTpLinkCommandCreator() : base("refresh") { }
    public override IRouterCommand CreateRouterCommand(RouterCommandContext context)
    {
        return new TpLinkRefreshRouterCommand(context.Router);
    }
}