namespace Router.TpLink.Status.Wlan;

public interface IWlanRouterStatusExtractor<in TWlanPageStatus, out TWlanRouterStatus>
    : IRouterStatusExtractor<TWlanPageStatus, TWlanRouterStatus> 
    where TWlanPageStatus: WlanPageStatus
    where TWlanRouterStatus: WlanRouterStatus 
{ }