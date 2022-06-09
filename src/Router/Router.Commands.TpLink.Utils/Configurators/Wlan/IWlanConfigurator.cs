using Router.Domain;
using Router.Domain.Properties;

namespace Router.Commands.TpLink.Utils;

public interface IWlanConfigurator : IConfigurator
{
    Task<WlanParameters> GetStatusAsync();
    Task EnableAsync();
    Task DisableAsync();
    Task SetSecurityAsync(Security security);
    Task SetSSIDAsync(string ssid);
}