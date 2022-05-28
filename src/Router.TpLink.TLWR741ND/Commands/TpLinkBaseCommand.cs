using Router.Commands;
using Router.Domain;

namespace Router.TpLink.Commands;

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