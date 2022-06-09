namespace Router.Commands.TpLink.Configurators.Router;

public abstract class BaseRouterConfigurator: BaseConfigurator, IRouterConfigurator
{
    protected BaseRouterConfigurator(IRouterHttpMessageSender messageSender) 
        : base(messageSender) 
    { }

    public abstract Task<bool> CheckConnectionAsync();
    public abstract Task RefreshAsync();
}