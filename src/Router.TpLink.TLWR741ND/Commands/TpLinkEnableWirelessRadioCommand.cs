namespace Router.TpLink.TLWR741ND.Commands;

public class TpLinkEnableWirelessRadioCommand : TpLinkSetWirelessRadioCommand 
{
    public TpLinkEnableWirelessRadioCommand(TpLinkRouter router) : base(router, true) 
    { }
}