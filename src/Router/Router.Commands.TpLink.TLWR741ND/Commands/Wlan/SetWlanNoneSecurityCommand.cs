using Router.Commands.TpLink.Configurators.Wlan;
using Router.Domain.Wlan;
using Router.Utils.Security;

namespace Router.Commands.TpLink.TLWR741ND.Commands.Wlan;

public class SetWlanNoneSecurityCommand : SetWlanSecurityCommand
{
    public SetWlanNoneSecurityCommand(IWlanConfigurator configurator)
        : base(configurator, new NoneSecurity())
    {
    }
}