using Router.Commands;
using Router.TpLink.CommandCreators;
using Router.TpLink.TLWR741ND.Commands;

namespace Router.TpLink.TLWR741ND.CommandCreators.Root;

internal class CheckConnectionTpLinkCommandCreator : SingleTpLinkCommandCreator
{
    public CheckConnectionTpLinkCommandCreator()
        : base("health")
    { }
    public override IRouterCommand CreateRouterCommand(RouterCommandContext context)
    {
        return new TpLinkHealthCheckCommand(context.Router);
    }
}