using Router.Commands.TpLink.CommandFactory;
using Router.Commands.TpLink.Configurators.Wlan;

namespace Router.Commands.TpLink.TLWR741ND.CommandFactory.Wlan;

public class WlanSecurityCompositeCommandFactory : CompositeTpLinkCommandFactory
{
    private static IEnumerable<TpLink.CommandFactory.TpLinkCommandFactory> GetWlanSecurityCommands(
        IWlanConfigurator wlan) =>
        new TpLink.CommandFactory.TpLinkCommandFactory[]
        {
            new GetWlanSecurityStatusCommandFactory(wlan), new SetPersonalSecurityCommandFactory(wlan),
            new SetEnterpriseSecurityCommandFactory(wlan), new SetWepSecurityCommandFactory(wlan),
            new SetNoneSecurityCommandFactory(wlan)
        };

    public WlanSecurityCompositeCommandFactory(IWlanConfigurator wlan)
        : base(GetWlanSecurityCommands(wlan), "security")
    {
    }
}