using Router.Commands;
using Router.TpLink.Commands;

namespace Router.TpLink.CommandFactories.Wlan;

internal class DisableWirelessRadioTpLinkCommandFactory : SingleTpLinkCommandFactory
{
    public DisableWirelessRadioTpLinkCommandFactory() : base("disable") { }
    public override IRouterCommand CreateRouterCommand(RouterCommandContext context)
    {
        return new TpLinkDisableWirelessRadioCommand(context.Router);
    }
}