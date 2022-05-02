using System.Net.Http.Headers;
using System.Text;
using Router.Domain;

namespace Router.Commands;

public abstract class BaseRouterCommand : IRouterCommand
{
    protected RouterParameters RouterParameters { get; }
    protected string AuthorizationHeaderBase64Encoded { get; }

    protected BaseRouterCommand(RouterParameters routerParameters)
    {
        RouterParameters = routerParameters;
        AuthorizationHeaderBase64Encoded =
            Convert.ToBase64String(Encoding.UTF8.GetBytes($"{RouterParameters.Username}:{RouterParameters.Password}"));
    }

    protected HttpRequestMessage GetRequestMessageBase(string path, string query = "", HttpMethod? method = null)
    {
        var uri = new UriBuilder(RouterParameters.GetAddress()) {Path = path, Query = query}.Uri;
        return new HttpRequestMessage(method ?? HttpMethod.Get, uri)
               {
                   Headers =
                   {
                       Referrer = RouterParameters.GetAddress(),
                       Authorization =
                           new AuthenticationHeaderValue("Basic", AuthorizationHeaderBase64Encoded)
                   }
               };
    }

    public abstract Task ExecuteAsync();
}