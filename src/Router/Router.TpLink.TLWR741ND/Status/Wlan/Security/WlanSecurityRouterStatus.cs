namespace Router.TpLink.TLWR741ND.Status.Wlan.Security;

public class WlanSecurityRouterStatus : WlanRouterStatus
{
    public WlanSecurityRouterStatus(Domain.Security security)
    {
        Security = security;
    }
    public Domain.Security Security { get; }
}