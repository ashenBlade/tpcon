using Router.Commands.TpLink.Configurators.Wlan;
using Router.Commands.TpLink.TLWR741ND.Commands.Wlan;

namespace Router.Commands.TpLink.TLWR741ND.CommandFactory.Wlan;

internal class EnableWirelessRadioCommandFactory : WlanSingleCommandFactory
{
    public EnableWirelessRadioCommandFactory(IWlanConfigurator wlan)
        : base(wlan)
    {
    }

    public override IRouterCommand CreateRouterCommand(RouterCommandContext context)
    {
        return new EnableWirelessRadioCommand(Wlan);
    }
}