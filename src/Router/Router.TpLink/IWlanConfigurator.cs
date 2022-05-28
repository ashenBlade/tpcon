using Router.Domain.RouterProperties;

namespace Router.TpLink;

public interface IWlanConfigurator
{
    public Task<WlanParameters> GetStatusAsync();
    public Task EnableAsync();
    public Task DisableAsync();
    public Task SetPasswordAsync(string password);
    public Task SetSsidAsync(string ssid);
}