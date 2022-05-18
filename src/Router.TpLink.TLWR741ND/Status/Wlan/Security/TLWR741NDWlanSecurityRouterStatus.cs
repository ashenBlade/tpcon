using Router.TpLink.Status.Wlan.Security;

namespace Router.TpLink.TLWR741ND.Status.Wlan.Security;

public class TLWR741NDWlanSecurityRouterStatus : WlanSecurityRouterStatus
{
    public TLWR741NDWlanSecurityRouterStatus(string password)
    {
        Password = password;
    }

    public string Password { get; }
}