using Router.Commands.Commands;
using Router.Domain;

namespace Router.Commands;

public abstract class RouterCommandFactory : IRouterCommandFactory
{
    protected RouterParameters RouterParameters { get; }

    protected RouterCommandFactory(RouterParameters routerParameters)
    {
        RouterParameters = routerParameters;
    }
    
    public abstract HealthCheckCommand CreateHealthCheckCommand();
    public abstract RefreshRouterCommand CreateRefreshRouterCommand();
    public abstract GetWlanStatusCommand CreateGetWlanStatusCommand();
    public abstract SetWlanSsidCommand CreateSetWlanSsidCommand(string ssid);
}