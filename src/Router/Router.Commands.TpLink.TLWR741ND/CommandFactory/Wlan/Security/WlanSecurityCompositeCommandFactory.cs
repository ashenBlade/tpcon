using Router.Commands.TpLink.CommandFactory;
using Router.Commands.TpLink.Configurators.Wlan;

namespace Router.Commands.TpLink.TLWR741ND.CommandFactory.Wlan.Security;

public class WlanSecurityCompositeCommandFactory : CompositeTpLinkCommandFactory
{
    private static IEnumerable<KeyValuePair<CommandFactoryInfo, Func<BaseTpLinkCommandFactory>>>
        GetWlanSecurityCommands(
        IWlanConfigurator wlan) =>
        new[]
        {
            Command("status", "Получить информацию о защите Wi-Fi",
                    () => new GetWlanSecurityStatusCommandFactory(wlan)),
            Command("personal", "Выставить защиту Wi-Fi технологией WPA/WPA2-Personal",
                    () => new SetPersonalSecurityCommandFactory(wlan)),
            Command("enterprise", "Выставить защиту Wi-Fi технологией WPA/WPA2-Enterprise",
                    () => new SetEnterpriseSecurityCommandFactory(wlan)),
            Command("wep", "Выставить защиту Wi-Fi технологией Wep",
                    () => new SetWepSecurityCommandFactory(wlan)),
            Command("none", "Убрать защиту Wi-Fi",
                    () => new SetNoneSecurityCommandFactory(wlan))
        };

    public WlanSecurityCompositeCommandFactory(IWlanConfigurator wlan)
        : base(GetWlanSecurityCommands(wlan))
    {
    }
}