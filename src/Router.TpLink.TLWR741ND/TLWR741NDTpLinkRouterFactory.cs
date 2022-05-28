using JsUtils.Implementation;
using Router.Domain;
using Router.TpLink.TLWR741ND.Status.Lan.NetworkCfg;
using Router.TpLink.TLWR741ND.Status.Wlan;
using Router.TpLink.TLWR741ND.Status.Wlan.Network;
using Router.TpLink.TLWR741ND.Status.Wlan.Security;

namespace Router.TpLink.TLWR741ND;

public class TLWR741NDTpLinkRouterFactory : ITpLinkRouterFactory
{
    public TpLinkRouter CreateRouter(RouterParameters parameters)
    {
        var messageSender = CreateMessageSender(parameters);
        var wlan = CreateWlanConfigurator(messageSender);
        var lan = CreateLanConfigurator(messageSender);
        return new TLWR741NDTpLinkRouter(parameters, messageSender, wlan, lan);
    }
    
    private static IRouterHttpMessageSender CreateMessageSender(RouterParameters parameters) =>
        new TLWR741NDTpLinkRouterHttpMessageSender(new HtmlScriptVariableExtractor(new HtmlScriptExtractor(), new ScriptVariableExtractor(new Tokenizer())),
                                                   parameters);

    private static TLWR741NDTpLinkLanConfigurator CreateLanConfigurator(IRouterHttpMessageSender messageSender)
    {
        return new TLWR741NDTpLinkLanConfigurator(messageSender, LanNetworkStatusExtractor);
    }

    private static LanNetworkRouterStatusExtractor LanNetworkStatusExtractor => new();

    private static TLWR741NDTpLinkWlanConfigurator CreateWlanConfigurator(IRouterHttpMessageSender messageSender)
    {
        return new TLWR741NDTpLinkWlanConfigurator(messageSender, WlanNetworkStatusExtractor, WlanSecurityStatusExtractor);
    }

    private static WlanSecurityRouterStatusExtractor WlanSecurityStatusExtractor => new();

    private static WlanNetworkRouterStatusExtractor WlanNetworkStatusExtractor => new();
}