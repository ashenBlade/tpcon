using Router.Commands.TpLink.Configurators.Wlan;

namespace Router.Commands.TpLink.TLWR741ND.Commands.Wlan;

public class TpLinkEnableWirelessRadioCommand : TpLinkSetWirelessRadioCommand 
{
    public TpLinkEnableWirelessRadioCommand(IWlanConfigurator configurator) 
        : base(configurator, true) 
    { }
}