namespace Router.Commands.TpLink.Utils;

public interface IRouterConfigurator: IConfigurator
{
    Task<bool> CheckConnectionAsync();
    Task RefreshAsync();
}