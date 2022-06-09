using Router.Commands.TpLink.Commands;
using Router.Commands.TpLink.Configurators.Wlan;

namespace Router.Commands.TpLink.TLWR741ND.Commands.Wlan;

public abstract class WlanTpLinkCommand : TpLinkCommand<IWlanConfigurator>
{
    protected IWlanConfigurator Wlan => Configurator;
    
    protected WlanTpLinkCommand(IWlanConfigurator configurator) : base(configurator)
    { }
}