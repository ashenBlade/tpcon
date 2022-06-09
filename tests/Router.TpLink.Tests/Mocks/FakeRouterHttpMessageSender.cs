using System.Collections.Generic;
using System.Threading.Tasks;
using JsTypes;
using Router.Commands.TpLink;
using Router.Domain;

namespace Router.TpLink.Tests.Mocks;

public class FakeRouterHttpMessageSender : IRouterHttpMessageSender
{
    public RouterConnectionParameters RouterConnectionParameters { get; set; }
    public Task<List<JsVariable>> SendMessageAndParseAsync(RouterHttpMessage message)
    {
        throw new MustNotBeCalledException();
    }

    public Task SendMessageAsync(RouterHttpMessage message)
    {
        throw new MustNotBeCalledException();
    }
}
