using Router.Domain.Exceptions;

namespace Router.TpLink.Commands;

public class TpLinkHealthCheckCommand : TpLinkBaseCommand
{
    public TpLinkHealthCheckCommand(TpLinkRouter router) : base(router)
    { }
    
    public override async Task ExecuteAsync()
    {
        if (!await Router.CheckConnectionAsync())
        {
            throw new RouterUnreachableException(RouterParameters.Address.ToString());
        }
        Console.WriteLine("Ok");
    }
}