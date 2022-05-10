namespace Router.Commands.TpLink.CommandFactories.Wlan;

internal class WlanCompositeTpLinkCommandCreator : CompositeTpLinkCommandCreator
{
    public WlanCompositeTpLinkCommandCreator() 
        : base(new []
               {
                   new GetWlanStatusCompositeCreator()
               }, 
               "wlan") 
    { }
}