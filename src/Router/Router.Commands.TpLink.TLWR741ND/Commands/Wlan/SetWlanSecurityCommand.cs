using Router.Commands.TpLink.Configurators.Wlan;
using Router.Domain.Wlan;
using Router.Utils.Security;

namespace Router.Commands.TpLink.TLWR741ND.Commands.Wlan;

public class SetWlanSecurityCommand : WlanCommand
{
    public Security Security { get; }

    protected SetWlanSecurityCommand(IWlanConfigurator configurator, Security security)
        : base(configurator)
    {
        Security = security;
    }

    public override Task ExecuteAsync()
    {
        return Wlan.SetSecurityAsync(Security);
    }
}