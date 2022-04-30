using System.Net;
using Router.Domain;
using Router.Domain.Exceptions;

namespace Router.Commands.Commands;

public class RefreshRouterCommand : RouterCommand
{
    public RefreshRouterCommand(RouterParameters routerParameters) 
        : base(routerParameters) 
    { }
    
    public override async Task ExecuteAsync()
    {
        using var client = new HttpClient();
        using var message = GetRequestMessageBase("/userRpm/SysRebootRpm.htm", "Reboot=Reboot");
        HttpResponseMessage response;
        try
        {
            response = await client.SendAsync(message);
        }
        catch (HttpRequestException)
        {
            throw new RouterUnreachableException(RouterParameters.GetAddress().ToString());
        }
        if (response.StatusCode is HttpStatusCode.Unauthorized or HttpStatusCode.Forbidden)
        {
            throw new InvalidRouterCredentialsException(RouterParameters.Username, RouterParameters.Password);
        }
    }
}