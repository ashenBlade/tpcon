namespace Router.Commands.TpLink.Commands;

public class TpLinkSetWirelessRadioCommand : TpLinkBaseCommand
{
    private bool Enable { get; }

    public TpLinkSetWirelessRadioCommand(TpLinkRouter router, bool enable) : base(router)
    {
        Enable = enable;
    }
    public override async Task ExecuteAsync()
    {
        if (Enable)
        {
            await Router.EnableWirelessRadioAsync();
        }
        else
        {
            await Router.DisableWirelessRadioAsync();
        }
    }
}