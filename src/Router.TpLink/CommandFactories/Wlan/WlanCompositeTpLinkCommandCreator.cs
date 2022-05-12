namespace Router.TpLink.CommandFactories.Wlan;

internal class WlanCompositeTpLinkCommandCreator : CompositeTpLinkCommandCreator
{
    public WlanCompositeTpLinkCommandCreator() 
        : base(new InternalTpLinkCommandCreator[]
               {
                   new GetWlanStatusCompositeCommandCreator(),
                   new EnableWirelessRadioTpLinkCommandCreator(),
                   new DisableWirelessRadioTpLinkCommandCreator(),
                   new SetWlanPasswordTpLinkCommandCreator(),
                   new SetWlanSsidTpLinkCommandCreator()
               }, 
               "wlan") 
    { }
}