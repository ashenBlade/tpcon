using Router.Commands.TpLink.CommandFactory;
using Router.Commands.TpLink.Configurators.Router;
using Router.Commands.TpLink.TLWR741ND.Commands.Router;

namespace Router.Commands.TpLink.TLWR741ND.CommandFactory.Root;

internal class RefreshTpLinkCommandFactory : SingleTpLinkCommandFactory
{
    private readonly IRouterConfigurator _configurator;
    public RefreshTpLinkCommandFactory(IRouterConfigurator configurator) : base("refresh")
    {
        _configurator = configurator;
    }
    public override IRouterCommand CreateRouterCommand(RouterCommandContext context)
    {
        return new TpLinkRefreshRouterCommand(_configurator);
    }
}