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
    
    public TpLinkGetWlanStatusCommand(TextWriter output, TpLinkRouter router) : base(router)
    {
        Output = output;
    }
    public override async Task ExecuteAsync()
    {
        var wlan = await Router.GetWlanParametersAsync();
        await Output.WriteLineAsync($"Enabled: {wlan.IsActive}\nPassword: {wlan.Password}\nSSID: {wlan.SSID}\nRouter IP address: {wlan.RouterAddress}");
    }
}