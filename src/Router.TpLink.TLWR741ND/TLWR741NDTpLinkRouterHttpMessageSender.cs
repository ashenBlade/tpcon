using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Web;
using JsTypes;
using JsUtils;
using Router.Domain;
using Router.Domain.Exceptions;

namespace Router.TpLink.TLWR741ND;

public class TLWR741NDTpLinkRouterHttpMessageSender : IRouterHttpMessageSender
{
    private readonly IJsVariableExtractor _jsVariableExtractor;
    private readonly HttpClient _client;

    public TLWR741NDTpLinkRouterHttpMessageSender(IJsVariableExtractor jsVariableExtractor, HttpClient client, RouterParameters routerParameters)
    {
        _jsVariableExtractor = jsVariableExtractor;
        _client = client;
        RouterParameters = routerParameters;
    }
    public RouterParameters RouterParameters { get; set; }

    private Uri GetUri(RouterHttpMessage message) => new UriBuilder()
                                                     {
                                                         Host = RouterParameters.Address.ToString(),
                                                         Path = message.Path,
                                                         Query = message.Query?
                                                                        .Select(p => $"{HttpUtility.UrlEncode(p.Key)}={HttpUtility.UrlEncode(p.Value)}")
                                                                        .Aggregate((s, n) => $"{s}&{n}") ?? string.Empty,
                                                         Scheme = "http",
                                                     }.Uri;
    
    private static string Base64Encode(string source) => 
        Convert.ToBase64String(Encoding.UTF8.GetBytes(source));
    
    private AuthenticationHeaderValue AuthorizationHeaderEncoded =>
        new("Basic",
            Base64Encode($"{RouterParameters.Username}:{RouterParameters.Password}"));
    
    private async Task<HttpResponseMessage> GetResponseFromRouterAsync(RouterHttpMessage routerMessage)
    {
        ArgumentNullException.ThrowIfNull(routerMessage);
        var requestUri = GetUri(routerMessage);
        using var message = new HttpRequestMessage(routerMessage.Method ?? HttpMethod.Get, requestUri)
                            {
                                Headers = {Authorization = AuthorizationHeaderEncoded, Referrer = requestUri}
                            };
        try
        {
            var response = await _client.SendAsync(message);
            if (response.StatusCode is HttpStatusCode.Forbidden 
                                    or HttpStatusCode.Unauthorized)
            {
                throw new InvalidRouterCredentialsException(RouterParameters.Username, RouterParameters.Password);
            }

            response.EnsureSuccessStatusCode();
            return response;
        }
        catch (HttpRequestException)
        {
            throw new RouterUnreachableException(RouterParameters.Address);
        }
    }

    private List<JsVariable> ExtractVariablesFromHtml(string script) 
        => _jsVariableExtractor.ExtractVariables(script)
                               .ToList();

    public async Task<List<JsVariable>> SendMessageAndParseAsync(RouterHttpMessage routerMessage)
    {
        var response = await GetResponseFromRouterAsync(routerMessage);
        var html = await response.Content.ReadAsStringAsync();
        return ExtractVariablesFromHtml(html);
    }

    public async Task SendMessageAsync(RouterHttpMessage routerMessage)
    {
        await GetResponseFromRouterAsync(routerMessage);
    }
}