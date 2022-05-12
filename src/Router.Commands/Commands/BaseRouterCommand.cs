using System.Text;
using Router.Domain;

namespace Router.Commands.Commands;

public abstract class BaseRouterCommand : IRouterCommand
{
    protected RouterParameters RouterParameters { get; }

    protected BaseRouterCommand(RouterParameters routerParameters)
    {
        RouterParameters = routerParameters;
    }

    public abstract Task ExecuteAsync();
}