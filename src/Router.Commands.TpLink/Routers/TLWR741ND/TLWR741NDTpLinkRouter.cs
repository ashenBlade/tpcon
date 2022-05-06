using System.Net;
using JsTypes;
using Router.Domain;
using Router.Domain.RouterProperties;

namespace Router.Commands.TpLink.Routers;

public class TLWR741NDTpLinkRouter : TpLinkRouter
{
    public TLWR741NDTpLinkRouter(RouterParameters routerParameters) 
        : base(routerParameters) 
    { }
    
    public override async Task<WlanParameters> GetWlanParametersAsync()
    {
        var wlanParametersArray =
            ( await GetRouterStatusAsync("userRpm/StatusRpm.htm") )
           .First(v => v.Name is "wlanPara").Value as JsArray;
        var isActive = ( wlanParametersArray[0] as JsNumber )!.Value == 1;
        var ssid = ( wlanParametersArray[1] as JsString )!.Value;
        var password = ((( await GetRouterStatusAsync("userRpm/WlanSecurityRpm.htm") )
                        .First(v => v.Name is "wlanPara")
                        .Value as JsArray)!
                        [9] as JsString)!.Value;
        return new WlanParameters(ssid, password, isActive);
    }
}