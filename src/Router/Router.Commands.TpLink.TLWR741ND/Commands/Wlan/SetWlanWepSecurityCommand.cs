using Router.Commands.TpLink.Configurators.Wlan;
using Router.Domain.Wlan;
using Router.Utils.Security;

namespace Router.Commands.TpLink.TLWR741ND.Commands.Wlan;

public class SetWlanWepSecurityCommand : SetWlanSecurityCommand
{
    public SetWlanWepSecurityCommand(IWlanConfigurator configurator, WepSecurity security)
        : base(configurator, security)
    {
    }
}