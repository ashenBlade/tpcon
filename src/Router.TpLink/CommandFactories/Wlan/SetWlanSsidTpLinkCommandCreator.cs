using Router.Commands;
using Router.Commands.Exceptions;
using Router.TpLink.Commands;

namespace Router.TpLink.CommandFactories.Wlan;

internal class SetWlanSsidTpLinkCommandCreator : SingleTpLinkCommandCreator
{
    public SetWlanSsidTpLinkCommandCreator() : base("ssid")
    { }

    public override IRouterCommand CreateRouterCommand(RouterCommandContext context)
    {
        if (!context.HasNextCommand)
        {
            throw new ArgumentValueExpectedException("ssid", context.Command.ToArray(), "SSID value expected");
        }

        var ssid = context.NextCommand;
        return new TpLinkSetWlanSsidCommand(context.Router, ssid);
    }
}