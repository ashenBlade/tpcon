using System.Net;
using Router.Commands.CommandLine.Exceptions;
using Router.Commands.TpLink.CommandFactory;
using Router.Commands.TpLink.Configurators.Lan;
using Router.Commands.TpLink.TLWR741ND.Commands.Lan;

namespace Router.Commands.TpLink.TLWR741ND.CommandFactory.Lan;

public class SetIpAddressLanCommandFactory : LanSingleCommandFactory
{
    public SetIpAddressLanCommandFactory(ILanConfigurator lan) : base(lan)
    {
    }

    public override IRouterCommand CreateRouterCommand(RouterCommandContext context)
    {
        var ip = context.CurrentCommand
              ?? throw new ArgumentValueExpectedException("IP Address", context.Command,
                                                          "IP Address is not provided");
        if (!IPAddress.TryParse(ip, out var address))
        {
            throw new IncorrectArgumentValueException("IP Address", ip, context.Command, "IP Address is not valid");
        }

        return new SetIpAddressLanCommand(Lan, address);
    }
}