using JsTypes;
using Router.TpLink.Status.Wlan.Security;

namespace Router.TpLink.TLWR741ND.Status.Wlan.Security;

public class TLWR741NDWlanSecurityPageStatus : WlanSecurityPageStatus
{
    public TLWR741NDWlanSecurityPageStatus(JsArray wlanPara)
    {
        WlanPara = wlanPara;
    }

    public JsArray WlanPara { get; }
}