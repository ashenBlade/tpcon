namespace Router.TpLink.TLWR741ND.Status.Wlan.Network;

public class WlanNetworkRouterStatus : WlanRouterStatus
{
    public WlanNetworkRouterStatus(string ssid, bool enabled)
    {
        SSID = ssid;
        Enabled = enabled;
    }

    public string SSID { get; }
    public bool Enabled { get; }
}