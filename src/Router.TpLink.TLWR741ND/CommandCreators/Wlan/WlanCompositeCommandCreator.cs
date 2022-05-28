using Router.TpLink.CommandCreators;

namespace Router.TpLink.TLWR741ND.CommandCreators.Wlan;

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