using JsTypes;

namespace Router.TpLink.TLWR741ND.Status.Wlan.Security;

public class WlanSecurityPageStatus : WlanPageStatus
{
    public WlanSecurityPageStatus(JsArray wlanPara)
    {
        WlanPara = wlanPara;
    }

    public JsArray WlanPara { get; }
}