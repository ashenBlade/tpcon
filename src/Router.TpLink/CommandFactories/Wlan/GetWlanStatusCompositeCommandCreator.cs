using Router.Commands;
using Router.TpLink.Commands;

namespace Router.TpLink.CommandFactories.Wlan;

internal class GetWlanStatusCompositeCommandCreator : SingleTpLinkCommandCreator
{
    public GetWlanStatusCompositeCommandCreator() 
        : base("status") 
    { }
    public override IRouterCommand CreateRouterCommand(RouterCommandContext context)
    {
        return new TpLinkGetWlanStatusCommand(Console.Out, context.Router);
    }
}