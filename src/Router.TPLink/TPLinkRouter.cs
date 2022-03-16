using System.Buffers.Text;
using System.Data;
using System.Net.Http.Headers;
using System.Text;
using JsUtils;
using Router.Domain;

namespace Router.TPLink;

public class TPLinkRouter : RemoteRouter
{
    protected IJsVariableExtractor JsVariableExtractor { get; }
    protected IScriptExtractor ScriptExtractor { get; }
    protected HttpClient HttpClient { get; }
    public TPLinkRouter(string username, 
                           string password, 
                           Uri address, 
                           IJsVariableExtractor jsVariableExtractor, 
                           IScriptExtractor scriptExtractor, 
                           HttpClient httpClient) 
        : base(username, password, address)
    {
        JsVariableExtractor = jsVariableExtractor;
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

    public override async Task RebootAsync()
    {
        var builder = new UriBuilder(Address)
                      {
                          Path = "/userRpm/SysRebootRpm.htm",
                          Query = "Reboot=Reboot"
                      }.Uri;
        
        var message = new HttpRequestMessage(HttpMethod.Get, builder)
                      {
                          Headers = 
                          { 
                              Referrer = Address, 
                              Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.UTF8.GetBytes($"{Username}:{Password}"))),
                          }
                      };
        await HttpClient.SendAsync(message);
    }
}