namespace Router.Commands.TpLink.Configurators.Router;

public interface IRouterConfigurator: IConfigurator
{
    Task<bool> CheckConnectionAsync();
    Task RefreshAsync();
}