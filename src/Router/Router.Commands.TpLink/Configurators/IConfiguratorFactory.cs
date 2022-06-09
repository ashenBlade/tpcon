using Router.Commands.TpLink.Configurators.Lan;
using Router.Commands.TpLink.Configurators.Router;
using Router.Commands.TpLink.Configurators.Wlan;

namespace Router.Commands.TpLink.Configurators;

public interface IConfiguratorFactory
{
    IRouterConfigurator CreateRouterConfigurator();
    IWlanConfigurator CreateWlanConfigurator();
    ILanConfigurator CreateLanConfigurator();
}