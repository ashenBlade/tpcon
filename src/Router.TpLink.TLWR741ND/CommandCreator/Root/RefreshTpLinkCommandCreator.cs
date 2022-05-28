using Router.Commands;
using Router.TpLink.CommandCreator;
using Router.TpLink.TLWR741ND.Commands;

namespace Router.TpLink.TLWR741ND.CommandCreator.Root;

internal class RefreshTpLinkCommandCreator : SingleTpLinkCommandCreator
{
    public RefreshTpLinkCommandCreator() : base("refresh") { }
    public override IRouterCommand CreateRouterCommand(RouterCommandContext context)
    {
        return new TpLinkRefreshRouterCommand(context.Router);
    }
}