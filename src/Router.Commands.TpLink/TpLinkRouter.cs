using Router.Domain;
using Router.Domain.Exceptions;
using Router.Domain.RouterProperties;

namespace Router.Commands.TpLink;

public abstract class TpLinkRouter
{
    public RouterParameters RouterParameters => MessageSender.RouterParameters;
    protected IRouterHttpMessageSender MessageSender { get; }

    protected TpLinkRouter(IRouterHttpMessageSender messageSender)
    {
        ArgumentNullException.ThrowIfNull(messageSender);
        MessageSender = messageSender;
    }

    public async Task RefreshAsync()
    {
        await MessageSender.SendMessageAsync("/userRpm/SysRebootRpm.htm");
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

    public abstract Task<WlanParameters> GetWlanParametersAsync();
    
    public abstract Task EnableWirelessRadioAsync();
    public abstract Task DisableWirelessRadioAsync();
    public abstract Task SetSsidAsync(string ssid);
    public abstract Task SetPasswordAsync(string password);
}