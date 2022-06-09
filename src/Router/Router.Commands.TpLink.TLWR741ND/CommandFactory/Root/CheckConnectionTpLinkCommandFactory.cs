using Router.Commands.TpLink.CommandFactory;
using Router.Commands.TpLink.Configurators.Router;
using Router.Commands.TpLink.TLWR741ND.Commands;
using Router.Commands.TpLink.TLWR741ND.Commands.Router;

namespace Router.Commands.TpLink.TLWR741ND.CommandFactory.Root;

internal class CheckConnectionTpLinkCommandFactory : SingleTpLinkCommandFactory
{
    private readonly IRouterConfigurator _configurator;

    public CheckConnectionTpLinkCommandFactory(IRouterConfigurator configurator)
        : base("health")
    {
        _configurator = configurator;
    }

    public override IRouterCommand CreateRouterCommand(RouterCommandContext context)
    {
        return new TpLinkHealthCheckCommand(_configurator, context.OutputWriter, context.OutputFormatter);
    }
}