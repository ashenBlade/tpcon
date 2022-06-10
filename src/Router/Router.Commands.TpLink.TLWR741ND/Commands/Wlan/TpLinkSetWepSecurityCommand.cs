using Router.Commands.TpLink.Configurators.Wlan;
using Router.Domain.Wlan;
using Router.Utils.Security;

namespace Router.Commands.TpLink.TLWR741ND.Commands.Wlan;

public class TpLinkSetWepSecurityCommand : TpLinkSetSecurityCommand
{
    public TpLinkSetWepSecurityCommand(IWlanConfigurator configurator, WepSecurity security)
        : base(configurator, security)
    {
    }
}