using Router.Commands;

namespace Router.TpLink.Commands;

public class TpLinkGetWlanStatusCommand : TpLinkBaseCommand
{
    private readonly IOutputFormatter _formatter;
    private readonly TextWriter _output;
    
    public TpLinkGetWlanStatusCommand(TextWriter output, TpLinkRouter router, IOutputFormatter formatter) : base(router)
    {
        _formatter = formatter;
        _output = output;
    }
    public override async Task ExecuteAsync()
    {
        var wlan = await Router.Wlan.GetStatusAsync();
        var result = _formatter.Format(wlan);
        await _output.WriteLineAsync(result);
    }
}