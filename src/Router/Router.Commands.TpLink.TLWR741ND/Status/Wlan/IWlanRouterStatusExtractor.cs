namespace Router.Commands.TpLink.TLWR741ND.Status.Wlan;

public interface IWlanRouterStatusExtractor<in TWlanPageStatus, out TWlanRouterStatus>
    : IRouterStatusExtractor<TWlanPageStatus, TWlanRouterStatus> 
    where TWlanPageStatus: WlanPageStatus
    where TWlanRouterStatus: WlanRouterStatus 
{ }