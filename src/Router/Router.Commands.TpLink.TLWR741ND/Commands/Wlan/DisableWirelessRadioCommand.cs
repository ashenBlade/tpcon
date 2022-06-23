using Router.Commands.TpLink.Configurators.Wlan;

namespace Router.Commands.TpLink.TLWR741ND.Commands.Wlan;

public class DisableWirelessRadioCommand : SetWirelessRadioCommand
{
    public DisableWirelessRadioCommand(IWlanConfigurator configurator)
        : base(configurator, false)
    {
    }
}