using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace TpLinkConsole.Core
{
    public class RouterController
    {
        private readonly Router _router;
        private readonly HttpClient _client;
        private readonly HttpRequestMessage _baseMessage;

        public RouterController(IPAddress routerAddress, string username, string password)
        {
            _router = new Router(routerAddress, username, password);
            _client = new HttpClient() {BaseAddress = new Uri($"http://{routerAddress}"),};
            _baseMessage = CreateBaseMessage();
        }

        private string ConvertToBase64(string origin)
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(origin));
        }

        private HttpRequestMessage CreateBaseMessage()
        {
            var authHeader = new AuthenticationHeaderValue("Basic", ConvertToBase64($"{_router.Username}:{_router.Password}"));
            var message = new HttpRequestMessage()
                          {
                              Headers =
                              {
                                  // Without referer header returns 'Non authorized'
                                  Referrer = new Uri($"http://{_router.RouterIPAddress}"),
                                  Authorization = authHeader
                              }
                          };
            return message;
        }

        private HttpRequestMessage CreateHttpRequestMessage(HttpMethod method, string url)
        {
            var message = new HttpRequestMessage(method, url)
                          {
                              Headers =
                              {
                                  Authorization = _baseMessage.Headers.Authorization,
                                  Referrer = _baseMessage.Headers.Referrer,
                              }
                          };
            return message;
        }

        public async Task<bool> TestConnectionAsync()
        {
            var response = await _client.SendAsync(CreateHttpRequestMessage(HttpMethod.Get, "/"));
            return response.IsSuccessStatusCode;
        }
    }
}