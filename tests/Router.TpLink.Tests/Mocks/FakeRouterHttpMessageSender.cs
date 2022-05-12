using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using JsTypes;
using Router.Domain;
using Xunit.Sdk;

namespace Router.TpLink.Tests;

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
