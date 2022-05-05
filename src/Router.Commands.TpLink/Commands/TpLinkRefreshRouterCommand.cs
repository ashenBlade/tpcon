using System.Net;
using Router.Commands.Commands;
using Router.Domain;
using Router.Domain.Exceptions;

namespace Router.Commands.TpLink.Commands;

public class TpLinkRefreshRouterCommand : RefreshRouterCommand
{
    private TpLinkRouter Router { get; }
    public TpLinkRefreshRouterCommand(RouterParameters routerParameters, TpLinkRouter router)
        : base(routerParameters)
    {
        Router = router;
    }
    
    public override async Task ExecuteAsync()
    {
        await Router.RefreshAsync();
    }
}