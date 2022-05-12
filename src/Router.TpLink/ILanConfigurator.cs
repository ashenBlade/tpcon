using Router.Domain.RouterProperties;

namespace Router.TpLink;

public interface ILanConfigurator
{
    public Task<LanParameters> GetStatusAsync();
}