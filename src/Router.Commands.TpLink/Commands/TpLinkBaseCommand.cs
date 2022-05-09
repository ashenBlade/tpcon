using Router.Commands.Commands;
using Router.Domain;

namespace Router.Commands.TpLink.Commands;

public abstract class TpLinkBaseCommand : BaseRouterCommand
{
    protected TpLinkRouter Router { get; }

    protected TpLinkBaseCommand(TpLinkRouter router) : base(router.RouterParameters)
    {
        Router = router;
    }
}