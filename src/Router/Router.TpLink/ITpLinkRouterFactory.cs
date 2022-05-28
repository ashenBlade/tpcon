using Router.Domain;

namespace Router.TpLink;

public interface ITpLinkRouterFactory
{
    TpLinkRouter CreateRouter(RouterParameters messageSender);
}