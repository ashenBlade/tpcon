using Router.Commands.TpLink.CommandFactory;
using Router.Commands.TpLink.Configurators.Wlan;

namespace Router.Commands.TpLink.TLWR741ND.CommandFactory.Wlan;

public abstract class WlanSingleCommandFactory : SingleTpLinkCommandFactory
{
    protected IWlanConfigurator Wlan { get; }

    protected WlanSingleCommandFactory(IWlanConfigurator wlan)
    {
        Wlan = wlan;
    }
}