using System.Net.Http.Headers;
using System.Text;
using JsUtils;
using Router.Domain;

namespace Router.TPLink;

public abstract class TPLinkRouter : RemoteRouter
{
    protected IJsParser JsParser { get; }
    protected IScriptExtractor ScriptExtractor { get; }
    protected HttpClient HttpClient { get; }
    protected TPLinkRouter(string username, string password, Uri address, IJsParser jsParser, IScriptExtractor scriptExtractor, HttpClient httpClient) 
        : base(username, password, address)
    {
        JsParser = jsParser;
        ScriptExtractor = scriptExtractor;
        HttpClient = httpClient;
    }

    private string? _hashedCredentials;

    private string HashedCredentials =>
        _hashedCredentials ??= Convert.ToBase64String(Encoding.UTF8.GetBytes($"{Username}:{Password}"));
    public override async Task<bool> CheckConnectionAsync()
    {
        var message = new HttpRequestMessage() {Method = HttpMethod.Get, RequestUri = Address};
        message.Headers.Authorization = new AuthenticationHeaderValue("Basic", HashedCredentials);
        try
        {
            var response = await HttpClient.SendAsync(message);
            response.EnsureSuccessStatusCode();
            return true;
        }
        
        catch (HttpRequestException ex)
        {
            return false;
        }
    }
}