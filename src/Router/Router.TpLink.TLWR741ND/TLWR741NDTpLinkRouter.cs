using Router.Domain;
using Router.Domain.Exceptions;

namespace Router.TpLink.TLWR741ND;

public class TLWR741NDTpLinkRouter : TpLinkRouter
{
    public TLWR741NDTpLinkRouter(RouterParameters routerParameters, IRouterHttpMessageSender messageSender, IWlanConfigurator wlan, ILanConfigurator lan) 
        : base(messageSender, routerParameters,  lan, wlan) 
    { }

    public override async Task RefreshAsync()
    {
        await MessageSender.SendMessageAsync(new RouterHttpMessage( "/userRpm/SysRebootRpm.htm", 
                                                                    new KeyValuePair<string, string>[]
                                                                    {
                                                                        new("Reboot", "Reboot")
                                                                    } ));
    }

    public override async Task<bool> CheckConnectionAsync()
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