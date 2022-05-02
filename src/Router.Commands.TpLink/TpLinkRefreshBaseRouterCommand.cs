using System.Net;
using Router.Commands.Commands;
using Router.Domain;
using Router.Domain.Exceptions;

namespace Router.Commands.TpLink;

public class TpLinkRefreshRouterCommand : RefreshRouterCommand
{
    public TpLinkRefreshRouterCommand(RouterParameters routerParameters) 
        : base(routerParameters) 
    { }
    
    public override async Task ExecuteAsync()
    {
        using var client = new HttpClient();
        using var message = GetRequestMessageBase("/userRpm/SysRebootRpm.htm", "Reboot=Reboot");
        try
        {
            using var response = await client.SendAsync(message);
            if (response.StatusCode is HttpStatusCode.Unauthorized or HttpStatusCode.Forbidden)
            {
                throw new InvalidRouterCredentialsException(RouterParameters.Username, RouterParameters.Password);
            }
        }
        catch (HttpRequestException)
        {
            throw new RouterUnreachableException(RouterParameters.GetAddress().ToString());
        }
    }
}