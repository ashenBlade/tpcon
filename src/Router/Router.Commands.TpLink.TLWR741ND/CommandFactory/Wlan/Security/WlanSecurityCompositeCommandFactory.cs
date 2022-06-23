using Router.Commands.TpLink.CommandFactory;
using Router.Commands.TpLink.Configurators.Wlan;

namespace Router.Commands.TpLink.TLWR741ND.CommandFactory.Wlan.Security;

public class WlanSecurityCompositeCommandFactory : CompositeTpLinkCommandFactory
{
    private static IEnumerable<KeyValuePair<string, Func<BaseTpLinkCommandFactory>>> GetWlanSecurityCommands(
        IWlanConfigurator wlan)
    {
        yield return Pair("status", () => new GetWlanSecurityStatusCommandFactory(wlan));
        yield return Pair("personal", () => new SetPersonalSecurityCommandFactory(wlan));
        yield return Pair("enterprise", () => new SetEnterpriseSecurityCommandFactory(wlan));
        yield return Pair("wep", () => new SetWepSecurityCommandFactory(wlan));
        yield return Pair("none", () => new SetNoneSecurityCommandFactory(wlan));

        KeyValuePair<string, Func<BaseTpLinkCommandFactory>>
            Pair(string name, Func<BaseTpLinkCommandFactory> factory) => new(name, factory);
    }

    public WlanSecurityCompositeCommandFactory(IWlanConfigurator wlan)
        : base(GetWlanSecurityCommands(wlan))
    {
    }
}