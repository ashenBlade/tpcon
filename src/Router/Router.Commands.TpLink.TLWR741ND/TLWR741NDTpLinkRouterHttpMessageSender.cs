using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Web;
using JsTypes;
using JsUtils;
using Router;
using Router.Commands.TpLink;
using Router.Domain;
using Router.Exceptions;

namespace Router.Commands.TpLink.TLWR741ND;

public class TLWR741NDTpLinkRouterHttpMessageSender : IRouterHttpMessageSender
{
    private readonly IJsVariableExtractor _jsVariableExtractor;

    public TLWR741NDTpLinkRouterHttpMessageSender(IJsVariableExtractor jsVariableExtractor, 
                                                  RouterConnectionParameters routerConnectionParameters)
    {
        _jsVariableExtractor = jsVariableExtractor;
        _routerConnectionParameters = routerConnectionParameters;
    }

    private readonly RouterConnectionParameters _routerConnectionParameters;

    private (Uri request, Uri referer) GetUris(RouterHttpMessage message)
    {
        var builder = new UriBuilder
                      {
                          Host = _routerConnectionParameters.Address.ToString(),
                          Path = message.Path,
                          Scheme = "http",
                      };
        var referer = builder.Uri;
        
        builder.Query = message.Query?
                               .Select(p => $"{HttpUtility.UrlEncode(p.Key)}={HttpUtility.UrlEncode(p.Value)}")
                               .Aggregate((s, n) => $"{s}&{n}")
                     ?? string.Empty;
        var request = builder.Uri;
        
        return (request, referer);
    }

    private static string Base64Encode(string source) => 
        Convert.ToBase64String(Encoding.UTF8.GetBytes(source));
    
    private AuthenticationHeaderValue AuthorizationHeaderEncoded =>
        new("Basic",
            Base64Encode($"{_routerConnectionParameters.Username}:{_routerConnectionParameters.Password}"));
    
    private async Task<HttpResponseMessage> GetResponseFromRouterAsync(RouterHttpMessage routerMessage)
    {
        ArgumentNullException.ThrowIfNull(routerMessage);
        var (requestUri, referer) = GetUris(routerMessage);
        using var message = new HttpRequestMessage(routerMessage.Method ?? HttpMethod.Get, requestUri)
                            {
                                Headers = {Authorization = AuthorizationHeaderEncoded, Referrer = referer}
                            };
        try
        {
            using var client = new HttpClient();
            var response = await client.SendAsync(message);
            if (response.StatusCode is HttpStatusCode.Forbidden 
                                    or HttpStatusCode.Unauthorized)
            {
                throw new InvalidRouterCredentialsException(_routerConnectionParameters.Address, 
                                                            _routerConnectionParameters.Username, 
                                                            _routerConnectionParameters.Password);
            }

            response.EnsureSuccessStatusCode();
            return response;
        }
        catch (HttpRequestException)
        {
            throw new RouterUnreachableException(_routerConnectionParameters.Address);
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