using Router.Commands;
using Router.TpLink.CommandCreator;
using Router.TpLink.TLWR741ND.Commands;

namespace Router.TpLink.TLWR741ND.CommandCreator.Wlan;

internal class DisableWirelessRadioTpLinkCommandCreator : SingleTpLinkCommandCreator
{
    public DisableWirelessRadioTpLinkCommandCreator() : base("disable") { }
    public override IRouterCommand CreateRouterCommand(RouterCommandContext context)
    {
        return new TpLinkDisableWirelessRadioCommand(context.Router);
    }
}