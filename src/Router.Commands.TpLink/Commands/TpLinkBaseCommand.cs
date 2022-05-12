using Router.Commands.Commands;
using Router.Domain;

namespace Router.Commands.TpLink.Commands;

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