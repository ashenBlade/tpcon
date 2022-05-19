namespace Router.TpLink.CommandFactories.Wlan;

internal class WlanCompositeCommandFactory : CompositeTpLinkCommandFactory
{
    public WlanCompositeCommandFactory() 
        : base(new InternalTpLinkCommandFactory[]
               {
                   new GetWlanStatusCommandFactory(),
                   new EnableWirelessRadioCommandFactory(),
                   new DisableWirelessRadioTpLinkCommandFactory(),
                   new SetWlanPasswordCommandFactory(),
                   new SetWlanSsidCommandFactory()
               }, 
               "wlan") 
    { }
}