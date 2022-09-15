using Router.Commands.TpLink.CommandFactory;
using Router.Commands.TpLink.Configurators.Wlan;
using Router.Commands.TpLink.TLWR741ND.CommandFactory.Wlan.Security;

namespace Router.Commands.TpLink.TLWR741ND.CommandFactory.Wlan;

internal class WlanCompositeCommandFactory : CompositeTpLinkCommandFactory
{
    public WlanCompositeCommandFactory(IWlanConfigurator wlan)
        : base(GetWlanCommands(wlan))
    {
    }

    private static IEnumerable<KeyValuePair<CommandFactoryInfo, Func<BaseTpLinkCommandFactory>>> GetWlanCommands(
        IWlanConfigurator wlan)
    {
        yield return new(new("status", "Получить информацию о беспроводном соединении роутера"),
                         () => new GetWlanStatusCommandFactory(wlan));
        yield return new(new("enable", "Включить Wi-Fi. Для применения нужна перезагрузка"),
                         () => new EnableWirelessRadioCommandFactory(wlan));
        yield return new(new("disable", "Выключить Wi-Fi. Для применения нужна перезагрузка"),
                         () => new DisableWirelessRadioTpLinkCommandFactory(wlan));
        yield return new(new("ssid", "Установить SSID роутера. То, под каким именем его видят другие"),
                         () => new SetWlanSsidCommandFactory(wlan));
        yield return new(new("security", "Информация о защите беспроводной сети"),
                         () => new WlanSecurityCompositeCommandFactory(wlan));
    }
}