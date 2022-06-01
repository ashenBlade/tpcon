using Router.Domain.RouterProperties;

namespace Router.TpLink.TLWR741ND.Utils;

public abstract class BaseWlanConfigurator : BaseConfigurator, IWlanConfigurator
{
    public abstract Task<WlanParameters> GetStatusAsync();
    public abstract Task EnableAsync();
    public abstract Task DisableAsync();
    public abstract Task SetPasswordAsync(string password);
    public abstract Task SetSsidAsync(string ssid);

    protected BaseWlanConfigurator(IRouterHttpMessageSender messageSender) 
        : base(messageSender)
    {
    }
}