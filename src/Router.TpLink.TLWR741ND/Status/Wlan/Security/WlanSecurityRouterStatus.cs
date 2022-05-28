namespace Router.TpLink.TLWR741ND.Status.Wlan.Security;

public class WlanSecurityRouterStatus : WlanRouterStatus
{
    public WlanSecurityRouterStatus(string password)
    {
        Password = password;
    }

    public string Password { get; }
}