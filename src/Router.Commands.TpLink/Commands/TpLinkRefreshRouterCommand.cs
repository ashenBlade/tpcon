using System.Net;
using Router.Commands.Commands;
using Router.Domain;
using Router.Domain.Exceptions;

namespace Router.Commands.TpLink.Commands;

public class TpLinkRefreshRouterCommand : TpLinkBaseCommand
{
    public TpLinkRefreshRouterCommand(TpLinkRouter router)
        : base(router)
    { }
    
    public override async Task ExecuteAsync()
    {
        await Router.RefreshAsync();
    }
}