using Router.Commands.TpLink.Commands;

namespace Router.Commands.TpLink.CommandFactories.Wlan;

internal class DisableWirelessRadioTpLinkCommandCreator : SingleTpLinkCommandCreator
{
    public DisableWirelessRadioTpLinkCommandCreator() : base("disable") { }
    public override IRouterCommand CreateRouterCommand(RouterCommandContext context)
    {
        return new TpLinkDisableWirelessRadioCommand(context.Router);
    }
}