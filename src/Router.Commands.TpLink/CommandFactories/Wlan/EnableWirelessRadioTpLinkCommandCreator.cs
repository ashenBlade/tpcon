using Router.Commands.TpLink.Commands;

namespace Router.Commands.TpLink.CommandFactories.Wlan;

internal class EnableWirelessRadioTpLinkCommandCreator : SingleTpLinkCommandCreator
{
    public EnableWirelessRadioTpLinkCommandCreator() : base("enable") { }
    public override IRouterCommand CreateRouterCommand(RouterCommandContext context)
    {
        return new TpLinkEnableWirelessRadioCommand(context.Router);
    }
}