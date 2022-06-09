using Router.Domain;
using Router.Domain.Properties;

namespace Router.Commands.TpLink.Utils;

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