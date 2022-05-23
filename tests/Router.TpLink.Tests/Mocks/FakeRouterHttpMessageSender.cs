using System.Collections.Generic;
using System.Threading.Tasks;
using JsTypes;
using Router.Domain;

namespace Router.TpLink.Tests.Mocks;

public class FakeRouterHttpMessageSender : IRouterHttpMessageSender
{
    public RouterParameters RouterParameters { get; set; }
    public Task<List<JsVariable>> SendMessageAndParseAsync(RouterHttpMessage message)
    {
        throw new MustNotBeCalledException();
    }

    public Task SendMessageAsync(RouterHttpMessage message)
    {
        throw new MustNotBeCalledException();
    }
}
