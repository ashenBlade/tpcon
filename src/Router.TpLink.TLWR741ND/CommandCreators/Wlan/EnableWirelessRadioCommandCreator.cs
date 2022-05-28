using Router.Commands;
using Router.TpLink.CommandCreators;
using Router.TpLink.TLWR741ND.Commands;

namespace Router.TpLink.TLWR741ND.CommandCreators.Wlan;

internal class EnableWirelessRadioCommandCreator : SingleTpLinkCommandCreator
{
    public EnableWirelessRadioCommandCreator() : base("enable") { }
    public override IRouterCommand CreateRouterCommand(RouterCommandContext context)
    {
        return new TpLinkEnableWirelessRadioCommand(context.Router);
    }
}