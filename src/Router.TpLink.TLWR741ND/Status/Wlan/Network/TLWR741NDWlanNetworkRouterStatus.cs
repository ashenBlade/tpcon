using Router.TpLink.Status.Wlan.Network;

namespace Router.TpLink.TLWR741ND.Status.Wlan;

public class TLWR741NDWlanNetworkRouterStatus : WlanNetworkRouterStatus
{
    public TLWR741NDWlanNetworkRouterStatus(string ssid, bool enabled)
    {
        SSID = ssid;
        Enabled = enabled;
    }

    public string SSID { get; }
    public bool Enabled { get; }
}