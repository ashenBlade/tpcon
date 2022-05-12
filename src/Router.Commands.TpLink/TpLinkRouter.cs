using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Web;
using JsTypes;
using JsUtils.Implementation;
using Router.Domain;
using Router.Domain.Exceptions;
using Router.Domain.RouterProperties;

namespace Router.Commands.TpLink;

public abstract class TpLinkRouter
{
    protected HttpClient Client { get; }
    public RouterParameters RouterParameters { get; }

    protected TpLinkRouter(RouterParameters routerParameters, HttpClient client)
    {
        ArgumentNullException.ThrowIfNull(client);
        Client = client;
        RouterParameters = routerParameters;
    }

    private Uri GetUriForRouter(string path, IEnumerable<KeyValuePair<string, string>>? query = null)
    {
        var builder = new UriBuilder() 
                      {
                          Scheme = "http",
                          Host = RouterParameters.Address.ToString(),
                          Path = path, 
                          Query = query?
                                 .Select(q => $"{HttpUtility.UrlEncode(q.Key)}={HttpUtility.UrlEncode(q.Value)}")
                                 .Aggregate((s, n) => $"{s}&{n}") ?? string.Empty
                      };
        return builder.Uri;
    }
    
    private async Task<HttpResponseMessage> GetResponseFromRouterAsync(string path, IEnumerable<KeyValuePair<string, string>>? query = null)
    {
        var requestUri = GetUriForRouter(path, query);
        using var message = new HttpRequestMessage(HttpMethod.Get, requestUri)
                            {
                                Headers = {
                                              Authorization = AuthorizationHeaderEncoded, 
                                              Referrer = requestUri
                                          }
                            };
        try
        {
            using var response = await Client.SendAsync(message);
            if (response.StatusCode is HttpStatusCode.Unauthorized
                                    or HttpStatusCode.Forbidden)
            {
                throw new InvalidRouterCredentialsException(RouterParameters.Username, RouterParameters.Password);
            }

            return response;
        }
        catch (HttpRequestException)
        {
            throw new RouterUnreachableException(RouterParameters.Address.ToString());
        }
    }
    
    protected async Task SendToRouterAsync(string path, IEnumerable<KeyValuePair<string, string>>? query = null)
    {
        ArgumentNullException.ThrowIfNull(path);
        await GetResponseFromRouterAsync(path, query);
    }

    /// <summary>
    /// Send GET method to router and parse incoming html document
    /// </summary>
    /// <param name="path">Absolute path to page without router address</param>
    /// <returns>Variables declared in received html document</returns>
    protected async Task<List<JsVariable>> GetRouterStatusAsync(string path)
    {
        ArgumentNullException.ThrowIfNull(path);
        var html = await ( await GetResponseFromRouterAsync(path) )
                        .Content
                        .ReadAsStringAsync();
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
        var uri = new UriBuilder(RouterParameters.GetUriAddress()) {Path = path, Query = query}.Uri;
        return new HttpRequestMessage(method ?? HttpMethod.Get, uri)
               {
                   Headers =
                   {
                       Referrer = RouterParameters.GetUriAddress(),
                       Authorization = AuthorizationHeaderEncoded
                   }
               };
    }

    protected HttpRequestMessage CreateRequestMessageBase(string path,
                                                          IEnumerable<KeyValuePair<string, string>>? query,
                                                          HttpMethod? method = null) =>
        CreateRequestMessageBase(path, 
                                 query?.Select(p => $"{p.Key}={p.Value}").Aggregate((s, n) => $"{s}&{n}") ?? string.Empty, 
                                 method);


    public async Task RefreshAsync()
    {
        using var message = CreateRequestMessageBase("/userRpm/SysRebootRpm.htm", "Reboot=Reboot");
        try
        {
            using var response = await Client.SendAsync(message);
            if (response.StatusCode is HttpStatusCode.Unauthorized 
                                    or HttpStatusCode.Forbidden)
            {
                throw new InvalidRouterCredentialsException(RouterParameters.Username, RouterParameters.Password);
            }
        }
        catch (HttpRequestException)
        {
            throw new RouterUnreachableException(RouterParameters.GetUriAddress().ToString());
        }
    }
    
    public async Task<bool> CheckConnectionAsync()
    {
        try
        {
            using var msg = CreateRequestMessageBase(string.Empty);
            using var response = await Client.SendAsync(msg);
            return true;
        }
        catch (HttpRequestException)
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