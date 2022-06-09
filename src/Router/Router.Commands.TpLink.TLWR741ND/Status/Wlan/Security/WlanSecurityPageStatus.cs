using JsTypes;

namespace Router.Commands.TpLink.TLWR741ND.Status.Wlan.Security;

public class WlanSecurityPageStatus : WlanPageStatus
{
    public WlanSecurityPageStatus(JsArray wlanPara, JsArray wlanList)
    {
        WlanPara = wlanPara;
        WlanList = wlanList;
    }

    public JsArray WlanPara { get; }
    public JsArray WlanList { get; }
}