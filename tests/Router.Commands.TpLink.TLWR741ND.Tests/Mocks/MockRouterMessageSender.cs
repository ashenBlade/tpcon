using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using JsTypes;
using Router.Commands.TpLink.TLWR741ND.Status;

namespace Router.Commands.TpLink.TLWR741ND.Tests.Mocks;

public class MockRouterMessageSender : IRouterHttpMessageSender
{
    public Task<List<JsVariable>> SendMessageAndParseAsync(RouterHttpMessage message)
    {
        return Task.FromResult(new List<JsVariable>(0));
    }

    public Task SendMessageAsync(RouterHttpMessage message)
    {
        return Task.CompletedTask;
    }
}