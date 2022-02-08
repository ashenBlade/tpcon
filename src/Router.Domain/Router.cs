using Router.Domain.RouterProperties;

namespace Router.Domain;

public abstract class Router
{
    public RouterStatistics Statistics { get; protected set; }
    public LanParameters LanParameters { get; protected set; }
    public WlanParameters WlanParameters { get; protected set; }

    public abstract Task RefreshAsync();
    public abstract Task RebootAsync();
}