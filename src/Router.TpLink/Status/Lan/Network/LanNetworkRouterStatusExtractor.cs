namespace Router.TpLink.Status.Lan.Network;

public abstract class LanNetworkRouterStatusExtractor<TLanNetworkPageStatus, TLanNetworkRouterStatus>
    : ILanRouterStatusExtractor<TLanNetworkPageStatus, TLanNetworkRouterStatus>
    where TLanNetworkPageStatus: LanNetworkPageStatus
    where TLanNetworkRouterStatus: LanNetworkRouterStatus
{
    public abstract TLanNetworkRouterStatus ExtractStatus(TLanNetworkPageStatus status);
}