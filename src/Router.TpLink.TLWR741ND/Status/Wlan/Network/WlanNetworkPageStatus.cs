using JsTypes;

namespace Router.TpLink.TLWR741ND.Status.Wlan.Network;

public class WlanNetworkPageStatus : WlanPageStatus
{
    public WlanNetworkPageStatus(JsArray wlanPara)
    {
        WlanPara = wlanPara;
    }

    public JsArray WlanPara { get; }
}