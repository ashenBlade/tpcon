namespace Router.Commands.TpLink.Commands;

public class TpLinkEnableWirelessRadioCommand : TpLinkSetWirelessRadioCommand 
{
    public TpLinkEnableWirelessRadioCommand(TpLinkRouter router) : base(router, true) 
    { }
}