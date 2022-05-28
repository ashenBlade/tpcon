using Router.Commands;
using Router.TpLink.CommandCreators;
using Router.TpLink.TLWR741ND.Commands;

namespace Router.TpLink.TLWR741ND.CommandCreators.Root;

internal class RefreshTpLinkCommandCreator : SingleTpLinkCommandCreator
{
    public RefreshTpLinkCommandCreator() : base("refresh") { }
    public override IRouterCommand CreateRouterCommand(RouterCommandContext context)
    {
        return new TpLinkRefreshRouterCommand(context.Router);
    }
}