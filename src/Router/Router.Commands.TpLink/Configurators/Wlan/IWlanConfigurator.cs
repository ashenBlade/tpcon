using Router.Domain.Wlan;

namespace Router.Commands.TpLink.Configurators.Wlan;

public interface IWlanConfigurator : IConfigurator
{
    Task<WlanParameters> GetStatusAsync();
    Task EnableAsync();
    Task DisableAsync();
    Task SetSecurityAsync(Security security);
    Task SetSSIDAsync(string ssid);
}