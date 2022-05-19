using Router.Commands;
using Router.TpLink.Commands;

namespace Router.TpLink.CommandFactories.Root;

internal class CheckConnectionTpLinkCommandFactory : SingleTpLinkCommandFactory
{
    public CheckConnectionTpLinkCommandFactory() 
        : base("health") 
    { }
    public override IRouterCommand CreateRouterCommand(RouterCommandContext context)
    {
        return new TpLinkHealthCheckCommand(context.Router);
    }
}