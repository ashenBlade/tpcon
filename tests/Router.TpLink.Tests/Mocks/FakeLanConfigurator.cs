using System.Threading.Tasks;
using Router.Domain.Properties;

namespace Router.TpLink.Tests.Mocks;

public class FakeLanConfigurator : ILanConfigurator
{
    public Task<LanParameters> GetStatusAsync()
    {
        throw new MustNotBeCalledException();
    }
}