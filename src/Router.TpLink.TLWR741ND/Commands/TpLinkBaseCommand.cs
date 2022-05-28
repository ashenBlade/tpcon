using Router.Commands;
using Router.Domain;

namespace Router.TpLink.TLWR741ND.Commands;

public abstract class TpLinkBaseCommand : IRouterCommand
{
    protected RouterParameters RouterParameters => Router.RouterParameters;
    protected TpLinkRouter Router { get; }

    protected TpLinkBaseCommand(TpLinkRouter router)
    {
        Router = router;
    }

    public abstract Task ExecuteAsync();
}