using Router.Commands.CommandLine.Exceptions;
using Router.Commands.TpLink.Configurators.Wlan;
using Router.Commands.TpLink.TLWR741ND.Commands.Wlan;

namespace Router.Commands.TpLink.TLWR741ND.CommandFactory.Wlan;

internal class SetWlanSsidCommandFactory : WlanSingleCommandFactory
{
    public SetWlanSsidCommandFactory(IWlanConfigurator wlan)
        : base(wlan)
    {
    }

    public override IRouterCommand CreateRouterCommand(RouterCommandContext context)
    {
        var ssid = context.CurrentCommand
                ?? throw new ArgumentValueExpectedException("SSID", context.Command.ToArray(), "SSID not provided");
        return new TpLinkSetWlanSsidCommand(Wlan, ssid);
    }
}