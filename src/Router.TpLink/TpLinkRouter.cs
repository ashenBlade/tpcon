using Router.Domain;
using Router.Domain.Exceptions;

namespace Router.TpLink;

public abstract class TpLinkRouter
{
    protected IRouterHttpMessageSender MessageSender { get; }
    public RouterParameters RouterParameters { get; }
    public ILanConfigurator Lan { get; }
    public IWlanConfigurator Wlan { get; }

    protected TpLinkRouter(IRouterHttpMessageSender messageSender, RouterParameters routerParameters, ILanConfigurator lan, IWlanConfigurator wlan)
    {
        ArgumentNullException.ThrowIfNull(lan);
        ArgumentNullException.ThrowIfNull(messageSender);
        ArgumentNullException.ThrowIfNull(wlan);
        MessageSender = messageSender;
        RouterParameters = routerParameters;
        Lan = lan;
        Wlan = wlan;
    }

    public async Task RefreshAsync()
    {
        await MessageSender.SendMessageAsync(new RouterHttpMessage( "/userRpm/SysRebootRpm.htm", 
                                                                    new KeyValuePair<string, string>[]
                                                                    {
                                                                        new("Reboot", "Reboot")
                                                                    } ));
    }
    
    public virtual async Task<bool> CheckConnectionAsync()
    {
        try
        {
            await MessageSender.SendMessageAsync(string.Empty);
            return true;
        }
        catch (RouterUnreachableException)
        {
            return false;
        }
    }
}