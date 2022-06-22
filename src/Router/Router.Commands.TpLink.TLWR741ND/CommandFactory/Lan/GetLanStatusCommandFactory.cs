using Router.Commands.TpLink.Configurators.Lan;
using Router.Commands.TpLink.TLWR741ND.Commands;

namespace Router.Commands.TpLink.TLWR741ND.CommandFactory.Lan;

internal class GetLanStatusCommandFactory : LanSingleCommandFactory
{
    public GetLanStatusCommandFactory(ILanConfigurator lan)
        : base(lan)
    {
    }

    public override IRouterCommand CreateRouterCommand(RouterCommandContext context)
    {
        return new TpLinkGetLanStatusCommand(Lan, context.OutputWriter, context.OutputFormatter);
    }
}