using System.Net;
using System.Net.Http.Headers;
using Router.Domain;
using Router.Domain.Exceptions;

namespace Router.Commands.Commands;

public abstract class HealthCheckCommand : BaseRouterCommand
{
    public HealthCheckCommand(RouterParameters routerParameters) : base(routerParameters) { }
}