using Router.Domain;
using Router.TpLink.TLWR741ND.Status.Lan.NetworkCfg;
using Router.TpLink.TLWR741ND.Status.Wlan;
using Router.TpLink.TLWR741ND.Status.Wlan.Security;

namespace Router.TpLink.TLWR741ND;

public class TLWR741NDTpLinkRouterFactory : ITpLinkRouterFactory
{
    public TpLinkRouter CreateRouter(IRouterHttpMessageSender messageSender)
    {
        var wlan = CreateWlanConfigurator(messageSender);
        var lan = CreateLanConfigurator(messageSender);
        return new TLWR741NDTpLinkRouter(messageSender, wlan, lan);
    }

    private static TLWR741NDTpLinkLanConfigurator CreateLanConfigurator(IRouterHttpMessageSender messageSender)
    {
        return new TLWR741NDTpLinkLanConfigurator(messageSender, LanNetworkStatusExtractor);
    }

    private static TLWR741NDLanRouterStatusExtractor LanNetworkStatusExtractor => new();

    private static TLWR741NDTpLinkWlanConfigurator CreateWlanConfigurator(IRouterHttpMessageSender messageSender)
    {
        return new TLWR741NDTpLinkWlanConfigurator(messageSender, WlanNetworkStatusExtractor, WlanSecurityStatusExtractor);
    }

    private static TLWR741NDWlanSecurityRouterStatusExtractor WlanSecurityStatusExtractor => new();

    private static TLWR741NDWlanNetworkRouterStatusExtractor WlanNetworkStatusExtractor => new();
}