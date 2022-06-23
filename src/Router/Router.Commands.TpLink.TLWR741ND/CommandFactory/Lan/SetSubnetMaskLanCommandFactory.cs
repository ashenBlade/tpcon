using Router.Commands.CommandLine.Exceptions;
using Router.Commands.TpLink.Configurators.Lan;
using Router.Commands.TpLink.TLWR741ND.Commands.Lan;
using Router.Domain.Lan;

namespace Router.Commands.TpLink.TLWR741ND.CommandFactory.Lan;

public class SetSubnetMaskLanCommandFactory : LanSingleCommandFactory
{
    public SetSubnetMaskLanCommandFactory(ILanConfigurator lan) : base(lan)
    {
    }

    public override IRouterCommand CreateRouterCommand(RouterCommandContext context)
    {
        var maskString = context.CurrentCommand
                      ?? throw new ArgumentValueExpectedException("Subnet mask", context.Command,
                                                                  "Specify subnet mask");
        if (!SubnetMask.TryParse(maskString, out var mask))
        {
            throw new IncorrectArgumentValueException("Subnet mask", maskString, context.Command,
                                                      "Incorrect subnet mask");
        }

        return new SetSubnetMaskLanCommand(Lan, mask);
    }
}