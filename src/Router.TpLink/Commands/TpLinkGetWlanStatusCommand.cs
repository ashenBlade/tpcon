using Router.Commands;
using Router.TpLink.Commands.DTO;

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
        var dto = new WlanStatusDTO()
                  {
                      Password = wlan.Password,
                      SSID = wlan.SSID,
                      IsActive = wlan.IsActive,
                      Address = wlan.RouterAddress.ToString()
                  };
        var result = _formatter.Format(dto);
        await _output.WriteLineAsync(result);
    }
}