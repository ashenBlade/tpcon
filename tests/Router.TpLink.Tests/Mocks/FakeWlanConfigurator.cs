using System.Threading.Tasks;
using Router.Domain.RouterProperties;

namespace Router.TpLink.Tests.Mocks;

public class FakeWlanConfigurator : IWlanConfigurator
{
    public Task<WlanParameters> GetStatusAsync()
    {
        throw new MustNotBeCalledException();
    }

    public Task EnableAsync()
    {
        throw new MustNotBeCalledException();
    }

    public Task DisableAsync()
    {
        throw new MustNotBeCalledException();
    }

    public Task SetPasswordAsync(string password)
    {
        throw new MustNotBeCalledException();
    }

    public Task SetSsidAsync(string ssid)
    {
        throw new MustNotBeCalledException();
    }
}
