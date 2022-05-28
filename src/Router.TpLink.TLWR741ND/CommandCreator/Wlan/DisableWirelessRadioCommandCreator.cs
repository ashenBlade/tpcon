using Router.Commands;
using Router.TpLink.Commands;

namespace Router.TpLink.CommandCreator.Wlan;

internal class DisableWirelessRadioTpLinkCommandCreator : SingleTpLinkCommandCreator
{
    public DisableWirelessRadioTpLinkCommandCreator() : base("disable") { }
    public override IRouterCommand CreateRouterCommand(RouterCommandContext context)
    {
        return new TpLinkDisableWirelessRadioCommand(context.Router);
    }
}