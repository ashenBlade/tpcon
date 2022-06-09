using Router.Commands.TpLink.Configurators.Wlan;
using Router.Domain.Wlan;

namespace Router.Commands.TpLink.TLWR741ND.Utils;

public abstract class BaseWlanConfigurator : BaseConfigurator, IWlanConfigurator
{
    public abstract Task<WlanParameters> GetStatusAsync();
    public abstract Task EnableAsync();
    public abstract Task DisableAsync();
    public abstract Task SetSecurityAsync(Security security);
    public abstract Task SetSSIDAsync(string ssid);

    protected BaseWlanConfigurator(IRouterHttpMessageSender messageSender) 
        : base(messageSender)
    { }
}