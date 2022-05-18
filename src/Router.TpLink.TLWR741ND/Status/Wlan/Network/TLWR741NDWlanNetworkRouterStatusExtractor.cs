using JsTypes;
using Router.TpLink.Status.Wlan.Network;

namespace Router.TpLink.TLWR741ND.Status.Wlan;

public class TLWR741NDWlanNetworkRouterStatusExtractor 
    : WlanNetworkRouterStatusExtractor<TLWR741NDWlanNetworkPageStatus, TLWR741NDWlanNetworkRouterStatus>
{
    public const int SSID = 3;
    public const int Enabled = 8;
    public override TLWR741NDWlanNetworkRouterStatus ExtractStatus(TLWR741NDWlanNetworkPageStatus status)
    {
        var wlan = status.WlanPara;
        var ssid = ( ( JsString ) wlan[SSID] ).Value;
        var enabled = ( ( JsNumber ) wlan[Enabled] ).Value == 1;
        return new TLWR741NDWlanNetworkRouterStatus(ssid, enabled);
    }
}