using Router.Commands;
using Router.TpLink.Commands;

namespace Router.TpLink.CommandCreator.Root;

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