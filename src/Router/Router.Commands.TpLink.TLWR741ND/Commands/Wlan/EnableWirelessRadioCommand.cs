using Router.Commands.TpLink.Configurators.Wlan;

namespace Router.Commands.TpLink.TLWR741ND.Commands.Wlan;

public class EnableWirelessRadioCommand : SetWirelessRadioCommand
{
    public EnableWirelessRadioCommand(IWlanConfigurator configurator)
        : base(configurator, true)
    {
    }
}