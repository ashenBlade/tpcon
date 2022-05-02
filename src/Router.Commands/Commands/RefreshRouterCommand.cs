using System.Net;
using Router.Domain;
using Router.Domain.Exceptions;

namespace Router.Commands.Commands;

public abstract class RefreshRouterCommand : BaseRouterCommand
{
    public RefreshRouterCommand(RouterParameters routerParameters) 
        : base(routerParameters) 
    { }
}