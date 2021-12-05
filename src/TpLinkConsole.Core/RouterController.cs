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
        public RouterController(IPAddress routerAddress, string username, string password)
        {
            _router = new Router(routerAddress, username, password);
            _client = new HttpClient() {BaseAddress = new Uri($"http://{routerAddress.ToString()}"),};
        }

        private HttpRequestMessage CreateMessage(HttpMethod method)
        {
            var message = new HttpRequestMessage()
                          {
                              Method = method,
                              Headers =
                              {
                                  Authorization =
                                      new AuthenticationHeaderValue("Basic",
                                                                    Convert.ToBase64String(Encoding.UTF8
                                                                                                   .GetBytes($"{_router.Username}:{_router.Password}"))),
                                  Referrer = new Uri($"http://{_router.RouterIPAddress}")
                              }
                          };
            return message;
        }

        public async Task<bool> TestConnectionAsync()
        {
            var response = await _client.SendAsync(CreateMessage(HttpMethod.Get));
            return response.IsSuccessStatusCode;
        }
    }
}