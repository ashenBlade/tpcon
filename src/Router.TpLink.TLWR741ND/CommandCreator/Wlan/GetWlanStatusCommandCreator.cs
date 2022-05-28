using Router.Commands;
using Router.TpLink.CommandCreator;
using Router.TpLink.TLWR741ND.Commands;

namespace Router.TpLink.TLWR741ND.CommandCreator.Wlan;

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