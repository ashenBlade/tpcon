using Router.Utils.Security;

namespace Router.Commands.TpLink.TLWR741ND.Status.Wlan.Security;

public class WlanSecurityRouterStatus : WlanRouterStatus
{
    public WlanSecurityRouterStatus(Domain.Wlan.Security currentSecurity, IEnumerable<KeyValuePair<string, string>> totalStatusValues)
    {
        CurrentSecurity = currentSecurity;
        TotalStatusValues = totalStatusValues;
    }
    public Domain.Wlan.Security CurrentSecurity { get; }
    public IEnumerable<KeyValuePair<string, string>> TotalStatusValues { get; }
}