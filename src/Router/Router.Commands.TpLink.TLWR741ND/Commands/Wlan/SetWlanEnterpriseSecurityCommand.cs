using Router.Commands.TpLink.Configurators.Wlan;
using Router.Domain.Wlan;
using Router.Utils.Security;

namespace Router.Commands.TpLink.TLWR741ND.Commands.Wlan;

public class SetWlanEnterpriseSecurityCommand : SetWlanSecurityCommand
{
    public SetWlanEnterpriseSecurityCommand(IWlanConfigurator configurator, EnterpriseSecurity security)
        : base(configurator, security)
    {
    }
}