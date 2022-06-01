using Router.Commands;
using Router.TpLink.CommandCreators;
using Router.TpLink.TLWR741ND.Commands;

namespace Router.TpLink.TLWR741ND.CommandCreator.Wlan;

public class GetWlanSecurityStatusCommandCreator : SingleTpLinkCommandCreator
{
    public GetWlanSecurityStatusCommandCreator() : base("status")
    { }

    public override IRouterCommand CreateRouterCommand(RouterCommandContext context)
    {
        return new TpLinkGetWlanSecurityStatusCommand(context.Router, Console.Out, context.OutputFormatter);
    }
}