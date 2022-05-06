using System.Net;
using System.Net.Http.Headers;
using System.Text;
using JsTypes;
using JsUtils.Implementation;
using Router.Domain;
using Router.Domain.Exceptions;
using Router.Domain.RouterProperties;

namespace Router.Commands.TpLink;

public abstract class TpLinkRouter
{
    public RouterParameters RouterParameters { get; }

    protected TpLinkRouter(RouterParameters routerParameters)
    {
        RouterParameters = routerParameters;
    }

    /// <summary>
    /// Send GET method to router and parse incoming html document
    /// </summary>
    /// <param name="path">Absolute path to page without router address</param>
    /// <returns>Variables declared in received html document</returns>
    protected async Task<List<JsVariable>> GetRouterStatusAsync(string path)
    {
        ArgumentNullException.ThrowIfNull(path);
        using var client = new HttpClient();
        var requestUri = new Uri(RouterParameters.GetAddress(), path);
        using var message = new HttpRequestMessage(HttpMethod.Get, requestUri)
                            {
                                Headers = {Authorization = AuthorizationHeaderEncoded, Referrer = requestUri}
                            };
        using var response = await client.SendAsync(message);
        if (response.StatusCode is HttpStatusCode.Unauthorized
                                or HttpStatusCode.Forbidden)
        {
            throw new InvalidRouterCredentialsException(RouterParameters.Username, RouterParameters.Password);
        }

        var html = await response.Content.ReadAsStringAsync();
        return new HtmlScriptVariableExtractor(new HtmlScriptExtractor(), new ScriptVariableExtractor(new Tokenizer()))
              .ExtractVariables(html)
              .ToList();
    }

    private static string Base64Encode(string source) => 
        Convert.ToBase64String(Encoding.UTF8.GetBytes(source));

    private AuthenticationHeaderValue AuthorizationHeaderEncoded =>
        new("Basic",
            Base64Encode($"{RouterParameters.Username}:{RouterParameters.Password}"));

    
    protected HttpRequestMessage CreateRequestMessageBase(string path, string query = "", HttpMethod? method = null)
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
        using var message = CreateRequestMessageBase("/userRpm/SysRebootRpm.htm", "Reboot=Reboot");
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
            using var msg = CreateRequestMessageBase(string.Empty);
            using var response = await client.SendAsync(msg);
            return true;
        }
        catch (HttpRequestException)
        {
            return false;
        }
    }

    public abstract Task<WlanParameters> GetWlanParametersAsync();
}