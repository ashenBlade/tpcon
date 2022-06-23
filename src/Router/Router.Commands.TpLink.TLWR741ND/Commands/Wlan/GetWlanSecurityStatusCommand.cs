using Router.Commands.TpLink.Configurators.Wlan;
using Router.Commands.TpLink.TLWR741ND.Commands.DisplayStatus.Wlan;

namespace Router.Commands.TpLink.TLWR741ND.Commands.Wlan;

public class GetWlanSecurityStatusCommand : WlanQueryCommand
{
    public GetWlanSecurityStatusCommand(IWlanConfigurator configurator, TextWriter writer, IOutputFormatter formatter)
        : base(configurator, writer, formatter)
    {
    }


    protected override async Task<TpLink.Commands.DisplayStatus> GetDisplayStatusAsync()
    {
        var security = ( await Wlan.GetStatusAsync() ).Security;
        return SecurityDisplayStatus.FromSecurity(security);
    }
}