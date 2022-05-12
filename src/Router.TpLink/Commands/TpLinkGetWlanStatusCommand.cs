namespace Router.TpLink.Commands;

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