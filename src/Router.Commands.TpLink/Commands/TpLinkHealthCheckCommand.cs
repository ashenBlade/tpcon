using Router.Commands.Commands;
using Router.Domain;
using Router.Domain.Exceptions;

namespace Router.Commands.TpLink.Commands;

public class TpLinkHealthCheckCommand : HealthCheckCommand
{
    private readonly TpLinkRouter _router;

    public TpLinkHealthCheckCommand(RouterParameters routerParameters,
                                    TpLinkRouter router) : base(routerParameters)
    {
        _router = router;
    }
    public override async Task ExecuteAsync()
    {
        if (!await _router.CheckConnectionAsync())
        {
            throw new RouterUnreachableException(_router.RouterParameters.Address.ToString());
        }
    }
}