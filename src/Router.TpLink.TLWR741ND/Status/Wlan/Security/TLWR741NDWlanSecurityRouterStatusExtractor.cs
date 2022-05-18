using JsTypes;
using Router.TpLink.Status.Wlan.Security;

namespace Router.TpLink.TLWR741ND.Status.Wlan.Security;

public class TLWR741NDWlanSecurityRouterStatusExtractor 
    : WlanSecurityRouterStatusExtractor<TLWR741NDWlanSecurityPageStatus, TLWR741NDWlanSecurityRouterStatus>
{
    private static class Wlan
    {
        public const int Password = 9;
    }
    public override TLWR741NDWlanSecurityRouterStatus ExtractStatus(TLWR741NDWlanSecurityPageStatus status)
    {
        var wlan = status.WlanPara;
        var password = ( ( JsString ) wlan[Wlan.Password] ).Value;
        return new TLWR741NDWlanSecurityRouterStatus(password);
    }
}