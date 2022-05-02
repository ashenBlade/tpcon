using Router.Commands.Commands;

namespace Router.Commands;

public interface IRouterCommandFactory
{
    HealthCheckCommand CreateHealthCheckCommand();
    RefreshRouterCommand CreateRefreshRouterCommand();
}