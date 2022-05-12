namespace Router.Commands.TpLink.Commands;

public class TpLinkDisableWirelessRadioCommand : TpLinkSetWirelessRadioCommand
{
    public TpLinkDisableWirelessRadioCommand(TpLinkRouter router) 
        : base(router, false) 
    { }
}