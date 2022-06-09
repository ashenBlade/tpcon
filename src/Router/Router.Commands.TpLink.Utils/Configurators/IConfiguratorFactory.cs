namespace Router.Commands.TpLink.Utils.Configurators;

public interface IConfiguratorFactory
{
    IRouterConfigurator CreateRouterConfigurator();
    IWlanConfigurator CreateWlanConfigurator();
    ILanConfigurator CreateLanConfigurator();
}