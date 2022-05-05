using System.Net;
using System.Text;
using JsTypes;
using JsUtils.Implementation;
using Router.Commands.Commands;
using Router.Domain;
using Router.Domain.Exceptions;
using Router.Domain.RouterProperties;

namespace Router.Commands.TpLink.Commands;

public class TpLinkGetWlanStatusCommand : GetWlanStatusCommand
{
    private TpLinkRouter Router { get; }
    
    public TpLinkGetWlanStatusCommand(RouterParameters routerParameters, TextWriter output, TpLinkRouter router) : base(routerParameters, output)
    {
        Router = router;
    }
    public override async Task ExecuteAsync()
    {
        var wlan = await Router.GetWlanParametersAsync();
        await Output.WriteLineAsync($"Enabled: {wlan.IsActive}");
        await Output.WriteLineAsync($"SSID: {wlan.SSID}");
    }
}