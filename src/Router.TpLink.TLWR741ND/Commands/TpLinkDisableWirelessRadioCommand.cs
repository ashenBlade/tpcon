namespace Router.TpLink.TLWR741ND.Commands;

public class TpLinkDisableWirelessRadioCommand : TpLinkSetWirelessRadioCommand
{
    public TpLinkDisableWirelessRadioCommand(TpLinkRouter router) 
        : base(router, false) 
    { }
}