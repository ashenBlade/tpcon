using Router.Domain.Wlan;

namespace Router.Commands.TpLink.Configurators.Wlan;

public abstract class BaseWlanConfigurator: BaseConfigurator, IWlanConfigurator
{
    protected BaseWlanConfigurator(IRouterHttpMessageSender messageSender) 
        : base(messageSender)
    { }

    public abstract Task<WlanParameters> GetStatusAsync();
    public abstract Task EnableAsync();
    public abstract Task DisableAsync();
    public abstract Task SetSecurityAsync(Security security);
    public abstract Task SetSSIDAsync(string ssid);
}