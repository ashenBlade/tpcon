using Router.Commands.Commands;
using Router.Commands.TpLink.Commands;
using Router.Commands.TpLink.Routers;
using Router.Domain;

namespace Router.Commands.TpLink;

public class TpLinkCommandFactory : RouterCommandFactory
{
    private TpLinkRouter Router { get; }
    public TpLinkCommandFactory(RouterParameters routerParameters)
        : base(routerParameters)
    {
        Router = new TLWR741NDTpLinkRouter(RouterParameters);
    }
    
    public override HealthCheckCommand CreateHealthCheckCommand()
    {
        return new TpLinkHealthCheckCommand(RouterParameters, Router);
    }

    public override RefreshRouterCommand CreateRefreshRouterCommand()
    {
        return new TpLinkRefreshRouterCommand(RouterParameters, Router);
    }

    public override GetWlanStatusCommand CreateGetWlanStatusCommand()
    {
        return new TpLinkGetWlanStatusCommand(RouterParameters, Console.Out, Router);
    }
}