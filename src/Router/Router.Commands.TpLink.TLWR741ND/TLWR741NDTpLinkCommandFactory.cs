using JsUtils.Implementation;
using Router.Commands.TpLink.CommandFactory;
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
    private static IEnumerable<KeyValuePair<CommandFactoryInfo, Func<BaseTpLinkCommandFactory>>> GetDefaultCommands(
        RouterConnectionParameters connectionParameters)
    {
        var sender = CreateMessageSender(connectionParameters);

        var wlan = CreateWlanConfigurator(sender);
        var lan = CreateLanConfigurator(sender);
        var router = CreateRouterConfigurator(sender);

        yield return new(new("health", "Проверить соединение с роутером"),
                         () => new CheckConnectionTpLinkCommandFactory(router));
        yield return new(new("refresh", "Перезагрузить роутер. "
                                      + "Перезагрузка нужна для применения установленных изменений"),
                         () => new RefreshTpLinkCommandFactory(router));
        yield return new(new("wlan", "Настройки беспроводной сети"),
                         () => new WlanCompositeCommandFactory(wlan));
        yield return new(new("lan", "Настройки локальной сети"),
                         () => new LanCompositeCommandFactory(lan));
    }

    private static RouterConfigurator CreateRouterConfigurator(IRouterHttpMessageSender sender)
    {
        return new RouterConfigurator(sender);
    }

    private static LanConfigurator CreateLanConfigurator(IRouterHttpMessageSender sender)
    {
        return new LanConfigurator(sender, new LanNetworkRouterStatusExtractor());
    }

    private static WlanConfigurator CreateWlanConfigurator(IRouterHttpMessageSender sender)
    {
        return new WlanConfigurator(sender,
                                    new WlanNetworkRouterStatusExtractor(),
                                    new WlanSecurityRouterStatusExtractor());
    }

    private static IRouterHttpMessageSender CreateMessageSender(RouterConnectionParameters connectionParameters)
    {
        return new TLWR741NDTpLinkRouterHttpMessageSender(new HtmlScriptVariableExtractor(new HtmlScriptExtractor(),
                                                                                          new
                                                                                              ScriptVariableExtractor(new
                                                                                                                          Tokenizer())),
                                                          connectionParameters);
    }

    public TLWR741NDTpLinkCommandFactory(RouterConnectionParameters connectionParameters)
        : base(GetDefaultCommands(connectionParameters), connectionParameters)
    {
    }
}