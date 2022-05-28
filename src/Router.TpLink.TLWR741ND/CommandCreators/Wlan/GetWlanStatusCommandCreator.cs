using Router.Commands;
using Router.TpLink.CommandCreators;
using Router.TpLink.TLWR741ND.Commands;

namespace Router.TpLink.TLWR741ND.CommandCreators.Wlan;

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