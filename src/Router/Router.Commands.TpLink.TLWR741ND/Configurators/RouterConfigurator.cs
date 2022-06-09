using Router.Commands.TpLink.Configurators.Router;
using Router.Exceptions;

namespace Router.Commands.TpLink.TLWR741ND.Configurators;

public class RouterConfigurator : BaseRouterConfigurator
{
    public RouterConfigurator(IRouterHttpMessageSender messageSender) 
        : base(messageSender)
    { }

    public override async Task<bool> CheckConnectionAsync()
    {
        try
        {
            await MessageSender.SendMessageAsync(string.Empty);
            return true;
        }
        catch (RouterConnectionException)
        {
            return false;
        }
    }

    public override async Task RefreshAsync()
    {
        await MessageSender.SendMessageAsync(new RouterHttpMessage( "/userRpm/SysRebootRpm.htm", 
                                                                    new KeyValuePair<string, string>[]
                                                                    {
                                                                        new("Reboot", "Reboot")
                                                                    } ));
    }
}