using Router.Domain;

namespace Router.TpLink.TLWR741ND;

public class TLWR741NDTpLinkRouterFactory : ITpLinkRouterFactory
{
    public TpLinkRouter CreateRouter(IRouterHttpMessageSender messageSender)
    {
        return new TLWR741NDTpLinkRouter(messageSender);
    }
}