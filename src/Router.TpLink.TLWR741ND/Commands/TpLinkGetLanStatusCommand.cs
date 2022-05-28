using Router.Commands;
using Router.TpLink.Commands.DisplayStatus;
using Router.TpLink.Commands.DTO;

namespace Router.TpLink.Commands;

public class TpLinkGetLanStatusCommand : TpLinkBaseCommand
{
    private readonly IOutputFormatter _formatter;
    private readonly TextWriter _output;

    public TpLinkGetLanStatusCommand(TpLinkRouter router, IOutputFormatter formatter, TextWriter output) 
        : base(router)
    {
        _formatter = formatter;
        _output = output;
    }

    public override async Task ExecuteAsync()
    {
        var lan = await Router.Lan.GetStatusAsync();
        var display = new LanDisplayStatus(lan.IpAddress, lan.MacAddress, lan.SubnetMask);
        var formatted = _formatter.Format(display);
        await _output.WriteLineAsync(formatted);
    }
}