using Router.Domain.RouterProperties;

namespace Router.TpLink;

public interface IWlanConfigurator : IConfigurator
{
    public Task<WlanParameters> GetStatusAsync();
    public Task EnableAsync();
    public Task DisableAsync();
    public Task SetPasswordAsync(string password);
    public Task SetSsidAsync(string ssid);
}