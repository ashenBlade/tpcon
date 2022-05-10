using Router.Commands.TpLink.Commands;
using Router.Commands.TpLink.Routers;

namespace Router.Commands.TpLink.CommandFactories.Wlan;

internal class GetWlanStatusCompositeCreator : SingleTpLinkCommandCreator
{
    public GetWlanStatusCompositeCreator() 
        : base("status") 
    { }
    public override IRouterCommand CreateRouterCommand(RouterCommandContext context)
    {
        return new TpLinkGetWlanStatusCommand(Console.Out, context.Router);
    }
}