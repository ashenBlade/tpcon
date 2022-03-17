using Router.Domain.RouterProperties;

namespace Router.Domain;

public abstract class Router
{
    public abstract Task<RouterStatistics> GetStatisticsAsync();
    public abstract Task<LanParameters> GetLanParametersAsync();
    public abstract Task<WlanParameters> GetWlanParametersAsync();

    public abstract Task RebootAsync();
}