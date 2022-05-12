using Router.Domain;

namespace Router.TpLink.Tests;

public class FakeTpLinkRouter : TpLinkRouter
{
    public new RouterParameters RouterParameters { get; }

    public FakeTpLinkRouter(RouterParameters routerParameters) : base(new FakeRouterHttpMessageSender(), new FakeLanConfigurator(), new FakeWlanConfigurator())
    {
        RouterParameters = routerParameters;
    }
}