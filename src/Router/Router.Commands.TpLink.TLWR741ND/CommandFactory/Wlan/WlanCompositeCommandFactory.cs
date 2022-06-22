using Router.Commands.TpLink.CommandFactory;
using Router.Commands.TpLink.Configurators.Wlan;

namespace Router.Commands.TpLink.TLWR741ND.CommandFactory.Wlan;

internal class WlanCompositeCommandFactory : CompositeTpLinkCommandFactory
{
    public WlanCompositeCommandFactory(IWlanConfigurator wlan)
        : base(GetWlanCommands(wlan))
    {
    }

    private static IEnumerable<KeyValuePair<string, Func<BaseTpLinkCommandFactory>>> GetWlanCommands(
        IWlanConfigurator wlan)
    {
        yield return new("status", () => new GetWlanStatusCommandFactory(wlan));
        yield return new("enable", () => new EnableWirelessRadioCommandFactory(wlan));
        yield return new("disable", () => new DisableWirelessRadioTpLinkCommandFactory(wlan));
        yield return new("ssid", () => new SetWlanSsidCommandFactory(wlan));
        yield return new("security", () => new WlanSecurityCompositeCommandFactory(wlan));
    }
}