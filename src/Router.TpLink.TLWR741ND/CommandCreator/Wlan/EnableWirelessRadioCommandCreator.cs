using Router.Commands;
using Router.TpLink.Commands;

namespace Router.TpLink.CommandCreator.Wlan;

internal class EnableWirelessRadioCommandCreator : SingleTpLinkCommandCreator
{
    public EnableWirelessRadioCommandCreator() : base("enable") { }
    public override IRouterCommand CreateRouterCommand(RouterCommandContext context)
    {
        return new TpLinkEnableWirelessRadioCommand(context.Router);
    }
}