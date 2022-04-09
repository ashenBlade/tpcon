using System.Text;
using Router.Domain;

namespace Router.Commands;

public abstract class RouterCommand : IRouterCommand
{
    protected RouterParameters RouterParameters { get; }
    protected string AuthorizationHeaderBase64Encoded { get; }
    
    protected RouterCommand(RouterParameters routerParameters)
    {
        RouterParameters = routerParameters;
        AuthorizationHeaderBase64Encoded =
            Convert.ToBase64String(Encoding.UTF8.GetBytes($"{RouterParameters.Username}:{RouterParameters.Password}"));
    }


    public abstract Task ExecuteAsync();
}