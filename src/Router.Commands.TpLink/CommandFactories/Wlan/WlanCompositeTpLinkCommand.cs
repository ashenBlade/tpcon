namespace Router.Commands.TpLink.CommandFactories.Wlan;

internal class WlanCompositeTpLinkCommand : CompositeTpLinkCommand
{
    public WlanCompositeTpLinkCommand() 
        : base(new []
               {
                   new GetWlanStatusCompositeTpLinkCommand()
               }, 
               "wlan") 
    { }

    public override void WriteHelpTo(TextWriter writer)
    {
        throw new NotImplementedException();
    }
}