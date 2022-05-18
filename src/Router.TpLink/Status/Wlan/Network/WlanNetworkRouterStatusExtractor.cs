namespace Router.TpLink.Status.Wlan.Network;

public abstract class WlanNetworkRouterStatusExtractor<TWlanNetworkPageStatus, TWlanNetworkRouterStatus> 
    : IWlanRouterStatusExtractor<TWlanNetworkPageStatus, TWlanNetworkRouterStatus>
    where TWlanNetworkPageStatus: WlanNetworkPageStatus
    where TWlanNetworkRouterStatus: WlanNetworkRouterStatus

{
    public abstract TWlanNetworkRouterStatus ExtractStatus(TWlanNetworkPageStatus status);
}