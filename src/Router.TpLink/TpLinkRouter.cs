using Router.Domain;
using Router.Domain.Exceptions;
using Router.Domain.RouterProperties;

namespace Router.TpLink;

public abstract class TpLinkRouter
{
    protected IRouterHttpMessageSender MessageSender { get; }
    public RouterParameters RouterParameters => MessageSender.RouterParameters;
    public ILanConfigurator Lan { get; }
    public IWlanConfigurator Wlan { get; }

    protected TpLinkRouter(IRouterHttpMessageSender messageSender, ILanConfigurator lan, IWlanConfigurator wlan)
    {
        ArgumentNullException.ThrowIfNull(messageSender);
        ArgumentNullException.ThrowIfNull(lan);
        ArgumentNullException.ThrowIfNull(wlan);
        MessageSender = messageSender;
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