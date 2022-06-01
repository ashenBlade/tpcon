using Router.Domain.RouterProperties;

namespace Router.TpLink;

public interface ILanConfigurator : IConfigurator
{
    public Task<LanParameters> GetStatusAsync();
}