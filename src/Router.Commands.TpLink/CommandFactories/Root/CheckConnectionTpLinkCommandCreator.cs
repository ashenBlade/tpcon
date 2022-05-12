using Router.Commands.TpLink.Commands;
using Router.Commands.TpLink.Routers;

namespace Router.Commands.TpLink.CommandFactories;

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