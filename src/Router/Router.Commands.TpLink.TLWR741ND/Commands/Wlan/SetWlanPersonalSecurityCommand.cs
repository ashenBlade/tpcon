using Router.Commands.TpLink.Configurators.Wlan;
using Router.Utils.Security;

namespace Router.Commands.TpLink.TLWR741ND.Commands.Wlan;

public class SetWlanPersonalSecurityCommand : SetWlanSecurityCommand
{
    public SetWlanPersonalSecurityCommand(IWlanConfigurator configurator, PersonalSecurity security)
        : base(configurator, security)
    {
    }
}