using Router.Commands.Commands;
using Router.Domain;
using Router.Domain.Exceptions;

namespace Router.Commands.TpLink.Commands;

public class TpLinkHealthCheckCommand : TpLinkBaseCommand
{
    public TpLinkHealthCheckCommand(TpLinkRouter router) : base(router)
    {
    }
    public override async Task ExecuteAsync()
    {
        if (!await Router.CheckConnectionAsync())
        {
            throw new RouterUnreachableException(RouterParameters.Address.ToString());
        }
    }
}