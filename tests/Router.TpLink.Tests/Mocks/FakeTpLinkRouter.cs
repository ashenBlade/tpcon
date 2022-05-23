using Router.Domain;

namespace Router.TpLink.Tests.Mocks;

public class FakeTpLinkRouter : TpLinkRouter
{
    public new RouterParameters RouterParameters { get; }

    public FakeTpLinkRouter(RouterParameters routerParameters) : base(new FakeRouterHttpMessageSender(), RouterParameters.Default,  new FakeLanConfigurator(), new FakeWlanConfigurator())
    {
        RouterParameters = routerParameters;
    }
}