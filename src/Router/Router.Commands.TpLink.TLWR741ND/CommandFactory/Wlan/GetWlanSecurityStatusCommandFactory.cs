using Router.Commands.TpLink.Configurators.Wlan;
using Router.Commands.TpLink.TLWR741ND.Commands.Wlan;

namespace Router.Commands.TpLink.TLWR741ND.CommandFactory.Wlan;

public class GetWlanSecurityStatusCommandFactory : WlanSingleCommandFactory
{
    public GetWlanSecurityStatusCommandFactory(IWlanConfigurator wlan)
        : base(wlan)
    {
    }

    public override IRouterCommand CreateRouterCommand(RouterCommandContext context)
    {
        return new GetWlanSecurityStatusCommand(Wlan, Console.Out, context.OutputFormatter);
    }
}