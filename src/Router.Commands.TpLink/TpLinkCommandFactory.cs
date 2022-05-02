using Router.Commands.Commands;
using Router.Domain;

namespace Router.Commands.TpLink;

public class TpLinkCommandFactory : RouterCommandFactory
{

    public TpLinkCommandFactory(RouterParameters routerParameters)
        : base(routerParameters)
    { }
    
    public override HealthCheckCommand CreateHealthCheckCommand()
    {
        return new TpLinkHealthCheckCommand(RouterParameters);
    }

    public override RefreshRouterCommand CreateRefreshRouterCommand()
    {
        return new TpLinkRefreshRouterCommand(RouterParameters);
    }
}