using Router.Commands.TpLink.CommandFactory;
using Router.Commands.TpLink.Configurators.Wlan;

namespace Router.Commands.TpLink.TLWR741ND.CommandFactory.Wlan;

internal class WlanCompositeCommandFactory : CompositeTpLinkCommandFactory
{
    public WlanCompositeCommandFactory(IWlanConfigurator wlan)
        : base(new TpLink.CommandFactory.TpLinkCommandFactory[]
               {
                   new GetWlanStatusCommandFactory(wlan),
                   new EnableWirelessRadioCommandFactory(wlan),
                   new DisableWirelessRadioTpLinkCommandFactory(wlan),
                   new SetWlanSsidCommandFactory(wlan),
                   new WlanSecurityCompositeCommandFactory(wlan)
               },
               "wlan")
    { }

    private static IEnumerable<TpLink.CommandFactory.TpLinkCommandFactory> GetDefaultFactories(IWlanConfigurator wlan) =>
        new TpLink.CommandFactory.TpLinkCommandFactory[]
        {
            new GetWlanStatusCommandFactory(wlan), 
            new EnableWirelessRadioCommandFactory(wlan),
            new DisableWirelessRadioTpLinkCommandFactory(wlan), 
            new SetWlanSsidCommandFactory(wlan), 
            new WlanSecurityCompositeCommandFactory(wlan)
        };
}