using Router.Commands.TpLink.Configurators.Wlan;

namespace Router.Commands.TpLink.TLWR741ND.Commands.Wlan;

public class TpLinkDisableWirelessRadioCommand : TpLinkSetWirelessRadioCommand
{
    public TpLinkDisableWirelessRadioCommand(IWlanConfigurator configurator) 
        : base(configurator, false) 
    { }
}