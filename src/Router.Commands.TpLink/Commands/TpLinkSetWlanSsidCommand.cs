using Router.Commands.Commands;
using Router.Domain;

namespace Router.Commands.TpLink.Commands;

public class TpLinkSetWlanSsidCommand : SetWlanSsidCommand
{
    public TpLinkRouter Router { get; }

    public TpLinkSetWlanSsidCommand(TpLinkRouter router, string ssid)
        : base(router.RouterParameters, ssid)
    {
        Router = router;
    }

    public override Task ExecuteAsync()
    {
        return Router.SetSsidAsync(Ssid);
    }
}