using System.Net;
using System.Text;
using JsTypes;
using JsUtils.Implementation;
using Router.Commands.Commands;
using Router.Domain;
using Router.Domain.Exceptions;
using Router.Domain.RouterProperties;

namespace Router.Commands.TpLink.Commands;

public class TpLinkGetWlanStatusCommand : TpLinkBaseCommand
{
    public TextWriter Output { get; }
    private TpLinkRouter Router { get; }
    
    public TpLinkGetWlanStatusCommand(TextWriter output, TpLinkRouter router) : base(router)
    {
        Output = output;
        Router = router;
    }
    public override async Task ExecuteAsync()
    {
        var wlan = await Router.GetWlanParametersAsync();
        await Output.WriteLineAsync($"Enabled: {wlan.IsActive}");
        await Output.WriteLineAsync($"Password: {wlan.Password}");
        await Output.WriteLineAsync($"SSID: {wlan.SSID}");
        await Output.WriteLineAsync($"Router IP address: {wlan.RouterAddress}");
    }
}