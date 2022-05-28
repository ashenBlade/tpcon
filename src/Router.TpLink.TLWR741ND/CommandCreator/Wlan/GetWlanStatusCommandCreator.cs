using Router.Commands;
using Router.TpLink.Commands;

namespace Router.TpLink.CommandCreator.Wlan;

internal class GetWlanStatusCommandCreator : SingleTpLinkCommandCreator
{
    public GetWlanStatusCommandCreator()
        : base("status")
    { }
    public override IRouterCommand CreateRouterCommand(RouterCommandContext context)
    {
        return new TpLinkGetWlanStatusCommand(Console.Out, context.Router, context.OutputFormatter);
    }
}