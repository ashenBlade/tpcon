using System.Threading.Tasks;
using Router.Domain;

namespace Router.TpLink.Tests.Mocks;

public class FakeTpLinkRouter : TpLinkRouter
{
    public new RouterParameters RouterParameters { get; }
    public override Task RefreshAsync()
    {
        throw new MustNotBeCalledException();
    }

    public override Task<bool> CheckConnectionAsync()
    {
        throw new MustNotBeCalledException();
    }

    public FakeTpLinkRouter(RouterParameters routerParameters) : base(new FakeRouterHttpMessageSender(), RouterParameters.Default,  new FakeLanConfigurator(), new FakeWlanConfigurator())
    {
        RouterParameters = routerParameters;
    }
}