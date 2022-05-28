namespace Router.TpLink.CommandCreator.Wlan;

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