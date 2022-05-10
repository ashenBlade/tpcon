namespace Router.Commands.TpLink.CommandFactories.Wlan;

internal class WlanCompositeTpLinkCommandCreator : CompositeTpLinkCommandCreator
{
    public WlanCompositeTpLinkCommandCreator() 
        : base(new InternalTpLinkCommandCreator[]
               {
                   new GetWlanStatusCompositeCreator(),
                   new EnableWirelessRadioTpLinkCommandCreator(),
                   new DisableWirelessRadioTpLinkCommandCreator()
               }, 
               "wlan") 
    { }
}