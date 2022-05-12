using System.Net;
using System.Web;
using JsTypes;
using Router.Domain.RouterProperties;

namespace Router.TpLink.TLWR741ND;

public class TLWR741NDTpLinkRouter : TpLinkRouter
{
    public TLWR741NDTpLinkRouter(IRouterHttpMessageSender sender, IWlanConfigurator wlan, ILanConfigurator lan) 
        : base(sender, lan, wlan) 
    { }
}