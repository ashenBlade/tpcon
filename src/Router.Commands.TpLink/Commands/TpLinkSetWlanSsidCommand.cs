namespace Router.Commands.TpLink.Commands;

public class TpLinkSetWlanSsidCommand : TpLinkBaseCommand
{
    private readonly string _ssid;

    public TpLinkSetWlanSsidCommand(TpLinkRouter router, string ssid)
        : base(router)
    {
        _ssid = ssid;
    }

    public override Task ExecuteAsync()
    {
        return Router.SetSsidAsync(_ssid);
    }
}