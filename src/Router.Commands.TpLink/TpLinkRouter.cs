using System.Net;
using System.Net.Http.Headers;
using System.Text;
using Router.Domain;
using Router.Domain.Exceptions;
using Router.Domain.RouterProperties;

namespace Router.Commands.TpLink;

public class TpLinkRouter
{
    public RouterParameters RouterParameters { get; }

    public TpLinkRouter(RouterParameters routerParameters)
    {
        RouterParameters = routerParameters;
    }

    private static string Base64Encode(string source)
    {
        return Convert.ToBase64String(Encoding.UTF8.GetBytes(source));
    }

    private AuthenticationHeaderValue AuthorizationHeaderEncoded =>
        new("Basic",
            Base64Encode($"{RouterParameters.Username}:{RouterParameters.Password}"));

    
    private HttpRequestMessage GetRequestMessageBase(string path, string query = "", HttpMethod? method = null)
    {
        var uri = new UriBuilder(RouterParameters.GetAddress()) {Path = path, Query = query}.Uri;
        return new HttpRequestMessage(method ?? HttpMethod.Get, uri)
               {
                   Headers =
                   {
                       Referrer = RouterParameters.GetAddress(),
                       Authorization = AuthorizationHeaderEncoded
                   }
               };
    }
    
    public async Task RefreshAsync()
    {
        using var client = new HttpClient();
        using var message = GetRequestMessageBase("/userRpm/SysRebootRpm.htm", "Reboot=Reboot");
        try
        {
            using var response = await client.SendAsync(message);
            if (response.StatusCode is HttpStatusCode.Unauthorized 
                                    or HttpStatusCode.Forbidden)
            {
                throw new InvalidRouterCredentialsException(RouterParameters.Username, RouterParameters.Password);
            }
        }
        catch (HttpRequestException)
        {
            throw new RouterUnreachableException(RouterParameters.GetAddress().ToString());
        }
    }

    public async Task<bool> CheckConnectionAsync()
    {
        using var client = new HttpClient();
        try
        {
            using var msg = GetRequestMessageBase(string.Empty);
            using var response = await client.SendAsync(msg);
            return true;
        }
        catch (HttpRequestException)
        {
            return false;
        }
    }

    public async Task<WlanParameters> GetWlanParametersAsync()
    {
        throw new NotImplementedException();
    }
}