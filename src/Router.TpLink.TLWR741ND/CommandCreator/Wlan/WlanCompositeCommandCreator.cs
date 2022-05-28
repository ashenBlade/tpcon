using Router.TpLink.CommandCreator;

namespace Router.TpLink.TLWR741ND.CommandCreator.Wlan;

internal class WlanCompositeCommandCreator : CompositeTpLinkCommandCreator
{
    public WlanCompositeCommandCreator()
        : base(new TpLinkCommandCreator[]
               {
                   new GetWlanStatusCommandCreator(),
                   new EnableWirelessRadioCommandCreator(),
                   new DisableWirelessRadioTpLinkCommandCreator(),
                   new SetWlanPasswordCommandCreator(),
                   new SetWlanSsidCommandCreator()
               },
               "wlan")
    { }
}