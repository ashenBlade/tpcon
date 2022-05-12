using Router.Commands.TpLink.Commands;

namespace Router.Commands.TpLink.CommandFactories.Root;

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