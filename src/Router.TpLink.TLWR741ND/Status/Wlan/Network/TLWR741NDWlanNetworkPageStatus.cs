using JsTypes;
using Router.TpLink.Status.Wlan.Network;

namespace Router.TpLink.TLWR741ND.Status.Wlan;

public class TLWR741NDWlanNetworkPageStatus : WlanNetworkPageStatus
{
    public TLWR741NDWlanNetworkPageStatus(JsArray wlanPara)
    {
        WlanPara = wlanPara;
    }

    public JsArray WlanPara { get; }
}