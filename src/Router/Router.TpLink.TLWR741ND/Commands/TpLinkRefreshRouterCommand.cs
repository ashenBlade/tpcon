namespace Router.TpLink.TLWR741ND.Commands;

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