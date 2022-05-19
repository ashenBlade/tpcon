using Router.Commands;
using Router.Commands.Exceptions;
using Router.TpLink.Commands;

namespace Router.TpLink.CommandFactories.Wlan;

internal class SetWlanPasswordCommandFactory : SingleTpLinkCommandFactory
{
    public SetWlanPasswordCommandFactory() : base("password")
    { }

    public override IRouterCommand CreateRouterCommand(RouterCommandContext context)
    {
        if (!context.HasNextCommand)
        {
            throw new ArgumentValueExpectedException("password", context.Command.ToArray(),
                                                     "Password to set not provided");
        }

        var password = context.NextCommand;
        return new TpLinkSetWlanPasswordCommand(context.Router, password);
    }
}