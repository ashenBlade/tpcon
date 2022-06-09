using System.Threading.Tasks;
using Router.Domain;

namespace Router.TpLink.Tests.Mocks;

public class FakeTpLinkRouter : TpLinkRouter
{
    public new RouterConnectionParameters RouterConnectionParameters { get; }
    public override Task RefreshAsync()
    {
        throw new MustNotBeCalledException();
    }

    public override Task<bool> CheckConnectionAsync()
    {
        throw new MustNotBeCalledException();
    }

    public FakeTpLinkRouter(RouterConnectionParameters routerConnectionParameters) : base(new FakeRouterHttpMessageSender(), RouterConnectionParameters.Default,  new FakeLanConfigurator(), new FakeWlanConfigurator())
    {
        RouterConnectionParameters = routerConnectionParameters;
    }
}