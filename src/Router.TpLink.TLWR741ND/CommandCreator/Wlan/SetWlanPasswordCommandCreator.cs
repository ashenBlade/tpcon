using Router.Commands;
using Router.Commands.Exceptions;
using Router.TpLink.CommandCreator;
using Router.TpLink.TLWR741ND.Commands;

namespace Router.TpLink.TLWR741ND.CommandCreator.Wlan;

internal class SetWlanPasswordCommandCreator : SingleTpLinkCommandCreator
{
    public SetWlanPasswordCommandCreator() : base("password")
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