namespace Router.TpLink.TLWR741ND.Commands;

public abstract class TpLinkSetWirelessRadioCommand : TpLinkBaseCommand
{
    private readonly bool _enable;

    public TpLinkSetWirelessRadioCommand(TpLinkRouter router, bool enable) : base(router)
    {
        _enable = enable;
    }
    public override async Task ExecuteAsync()
    {
        await ( _enable
                    ? Router.Wlan.EnableAsync()
                    : Router.Wlan.DisableAsync() );
    }
}