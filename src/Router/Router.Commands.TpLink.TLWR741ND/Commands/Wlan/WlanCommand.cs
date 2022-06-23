using Router.Commands.TpLink.Commands;
using Router.Commands.TpLink.Configurators.Wlan;

namespace Router.Commands.TpLink.TLWR741ND.Commands.Wlan;

public abstract class WlanCommand : TpLinkCommand<IWlanConfigurator>
{
    protected IWlanConfigurator Wlan => Configurator;

    protected WlanCommand(IWlanConfigurator configurator) : base(configurator)
    {
    }
}