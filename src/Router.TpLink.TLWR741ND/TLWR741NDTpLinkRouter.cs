using System.Net;
using System.Web;
using JsTypes;
using JsUtils.Implementation;
using Router.Domain;
using Router.Domain.RouterProperties;

namespace Router.TpLink.TLWR741ND;

public class TLWR741NDTpLinkRouter : TpLinkRouter
{
    public TLWR741NDTpLinkRouter(RouterParameters routerParameters, IRouterHttpMessageSender messageSender, IWlanConfigurator wlan, ILanConfigurator lan) 
        : base(messageSender, routerParameters,  lan, wlan) 
    { }
}