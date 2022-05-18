namespace Router.TpLink.Status.Wlan.Security;

public abstract class WlanSecurityRouterStatusExtractor<TWlanSecurityPageStatus, TWlanSecurityRouterStatus> 
    : IWlanRouterStatusExtractor<TWlanSecurityPageStatus, TWlanSecurityRouterStatus> 
    where TWlanSecurityPageStatus : WlanSecurityPageStatus 
    where TWlanSecurityRouterStatus: WlanSecurityRouterStatus

{
    public abstract TWlanSecurityRouterStatus ExtractStatus(TWlanSecurityPageStatus status);
}