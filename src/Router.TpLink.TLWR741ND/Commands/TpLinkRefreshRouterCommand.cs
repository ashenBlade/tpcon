namespace Router.TpLink.Commands;

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