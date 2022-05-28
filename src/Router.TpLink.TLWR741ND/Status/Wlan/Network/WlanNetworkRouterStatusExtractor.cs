using JsTypes;

namespace Router.TpLink.TLWR741ND.Status.Wlan.Network;

public class WlanNetworkRouterStatusExtractor
    : IWlanRouterStatusExtractor<WlanNetworkPageStatus, WlanNetworkRouterStatus>
{
    public const int SSID = 3;
    public const int Enabled = 8;
    
    public WlanNetworkRouterStatus ExtractStatus(WlanNetworkPageStatus status)
    {
        var wlan = status.WlanPara;
        var ssid = ( ( JsString ) wlan[SSID] ).Value;
        var enabled = ( ( JsNumber ) wlan[Enabled] ).Value == 1;
        return new WlanNetworkRouterStatus(ssid, enabled);
    }
}