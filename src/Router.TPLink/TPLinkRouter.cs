using System.Buffers.Text;
using System.Data;
using System.Net.Http.Headers;
using System.Text;
using JsUtils;
using Router.Domain;
using Router.Domain.RouterProperties;

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

    private HttpRequestMessage GetHttpRequestMessage(string path, string query = "")
    {
        var uri = new UriBuilder(Address) {Path = path, Query = query}.Uri;
        return new HttpRequestMessage(HttpMethod.Get, uri)
               {
                   Headers =
                   {
                       Referrer = Address,
                       Authorization =
                           new AuthenticationHeaderValue("Basic",
                                                         Convert.ToBase64String(Encoding.UTF8
                                                                                        .GetBytes($"{Username}:{Password}")))
                   }
               };
    }
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
        var message = GetHttpRequestMessage("/userRpm/SysRebootRpm.htm", "Reboot=Reboot");
        await HttpClient.SendAsync(message);
    }

    public override Task<LanParameters> GetLanParametersAsync()
    {
        throw new NotImplementedException();
    }

    public override Task<WlanParameters> GetWlanParametersAsync()
    {
        throw new NotImplementedException();
    }

    public override Task<RouterStatistics> GetStatisticsAsync()
    {
        throw new NotImplementedException();
    }
}