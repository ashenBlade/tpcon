using JsTypes;

namespace Router.TpLink.TLWR741ND.Status.Wlan.Security;

public  class WlanSecurityRouterStatusExtractor 
    : IWlanRouterStatusExtractor<WlanSecurityPageStatus, WlanSecurityRouterStatus>

{
    public const int Password = 9;
    
    public WlanSecurityRouterStatus ExtractStatus(WlanSecurityPageStatus status)
    {
        var wlan = status.WlanPara;
        var password = ( ( JsString ) wlan[Password] ).Value;
        return new WlanSecurityRouterStatus(password);
    }
}