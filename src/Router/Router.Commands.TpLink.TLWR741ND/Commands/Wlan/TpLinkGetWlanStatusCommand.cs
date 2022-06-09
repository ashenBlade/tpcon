using Router.Commands.TpLink.Configurators.Wlan;
using Router.Commands.TpLink.TLWR741ND.Commands.DisplayStatus.Wlan;

namespace Router.Commands.TpLink.TLWR741ND.Commands.Wlan;

public class TpLinkGetWlanStatusCommand : WlanTpLinkQueryCommand
{
    public TpLinkGetWlanStatusCommand(IWlanConfigurator configurator, TextWriter output, IOutputFormatter formatter) 
        : base(configurator, output, formatter)
    { }

    protected override async Task<TpLink.Commands.DisplayStatus> GetDisplayStatusAsync()
    {
        var wlan = await Wlan.GetStatusAsync();
        return new WlanDisplayStatus(wlan);
    }
}