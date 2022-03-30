using Router.Domain;

namespace Router.Commands;

public abstract class BaseRouterCommand : IRouterCommand
{
    protected RouterParameters RouterParameters { get; private set; }
    
    protected BaseRouterCommand(RouterParameters routerParameters)
    {
        RouterParameters = routerParameters;
    }
    
    public abstract Task ExecuteAsync();
}