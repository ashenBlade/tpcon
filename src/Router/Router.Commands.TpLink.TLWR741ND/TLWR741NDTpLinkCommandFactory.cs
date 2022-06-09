using JsUtils.Implementation;
using Router.Commands.TpLink.TLWR741ND.CommandFactory.Lan;
using Router.Commands.TpLink.TLWR741ND.CommandFactory.Root;
using Router.Commands.TpLink.TLWR741ND.CommandFactory.Wlan;
using Router.Commands.TpLink.TLWR741ND.Configurators;
using Router.Commands.TpLink.TLWR741ND.Status.Lan.NetworkCfg;
using Router.Commands.TpLink.TLWR741ND.Status.Wlan.Network;
using Router.Commands.TpLink.TLWR741ND.Status.Wlan.Security;
using Router.Domain;

namespace Router.Commands.TpLink.TLWR741ND;


public class TLWR741NDTpLinkCommandFactory : TpLinkCommandFactory
{
    private static IEnumerable<TpLink.CommandFactory.TpLinkCommandFactory> GetDefaultCommands(RouterConnectionParameters connectionParameters)
    {
        var sender = new TLWR741NDTpLinkRouterHttpMessageSender(new HtmlScriptVariableExtractor(new HtmlScriptExtractor(), new ScriptVariableExtractor(new Tokenizer())), connectionParameters);
        
        var wlan = new WlanConfigurator(sender, 
                                        new WlanNetworkRouterStatusExtractor(),
                                        new WlanSecurityRouterStatusExtractor());
        var lan = new LanConfigurator(sender, 
                                      new LanNetworkRouterStatusExtractor());
        var router = new RouterConfigurator(sender);
        return new TpLink.CommandFactory.TpLinkCommandFactory[]
               {
                   new CheckConnectionTpLinkCommandFactory(router), 
                   new RefreshTpLinkCommandFactory(router),
                   new WlanCompositeCommandFactory(wlan), 
                   new LanCompositeCommandFactory(lan)
               };
    }

    public TLWR741NDTpLinkCommandFactory(RouterConnectionParameters connectionParameters)
        : base(GetDefaultCommands(connectionParameters), connectionParameters)
    { }
}