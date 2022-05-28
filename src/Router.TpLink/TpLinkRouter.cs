using Router.Domain;
using Router.Domain.Exceptions;

namespace Router.TpLink;

public abstract class TpLinkRouter
{
    protected IRouterHttpMessageSender MessageSender { get; }
    public RouterParameters RouterParameters { get; }
    public ILanConfigurator Lan { get; }
    public IWlanConfigurator Wlan { get; }

    protected TpLinkRouter(IRouterHttpMessageSender messageSender, 
                           RouterParameters routerParameters, 
                           ILanConfigurator lan, 
                           IWlanConfigurator wlan)
    {
        ArgumentNullException.ThrowIfNull(lan);
        ArgumentNullException.ThrowIfNull(messageSender);
        ArgumentNullException.ThrowIfNull(wlan);
        MessageSender = messageSender;
        RouterParameters = routerParameters;
        Lan = lan;
        Wlan = wlan;
    }

    public abstract Task RefreshAsync();
    public abstract Task<bool> CheckConnectionAsync();
}