using Router.Commands;
using Router.TpLink.Commands;

namespace Router.TpLink.CommandFactories.Wlan;

internal class EnableWirelessRadioCommandFactory : SingleTpLinkCommandFactory
{
    public EnableWirelessRadioCommandFactory() : base("enable") { }
    public override IRouterCommand CreateRouterCommand(RouterCommandContext context)
    {
        return new TpLinkEnableWirelessRadioCommand(context.Router);
    }
}