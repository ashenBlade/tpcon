using Router.Commands;
using Router.TpLink.TLWR741ND.Commands.DisplayStatus;

namespace Router.TpLink.TLWR741ND.Commands;

public class TpLinkGetWlanStatusCommand : TpLinkBaseCommand
{
    private readonly IOutputFormatter _formatter;
    private readonly TextWriter _output;
    
    public TpLinkGetWlanStatusCommand(TextWriter output, TpLinkRouter router, IOutputFormatter formatter) 
        : base(router)
    {
        _formatter = formatter;
        _output = output;
    }
    
    public override async Task ExecuteAsync()
    {
        var wlan = await Router.Wlan.GetStatusAsync();
        var display = new WlanDisplayStatus(wlan.Password, wlan.SSID, wlan.IsActive);
        var result = _formatter.Format(display);
        await _output.WriteLineAsync(result);
    }
}