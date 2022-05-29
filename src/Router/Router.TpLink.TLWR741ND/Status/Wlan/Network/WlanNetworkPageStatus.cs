using JsTypes;

namespace Router.TpLink.TLWR741ND.Status.Wlan.Network;

public class WlanNetworkPageStatus : WlanPageStatus
{
    public WlanNetworkPageStatus(JsArray wlanPara, JsArray rateTable)
    {
        WlanPara = wlanPara;
        RateTable = rateTable;
    }

    public JsArray WlanPara { get; }
    public JsArray RateTable { get; }
}