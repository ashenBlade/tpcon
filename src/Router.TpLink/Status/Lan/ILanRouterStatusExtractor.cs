namespace Router.TpLink.Status.Lan;

public interface ILanRouterStatusExtractor<TLanPageStatus, TLanRouterStatus> 
    : IRouterStatusExtractor<TLanPageStatus, TLanRouterStatus>
    where TLanPageStatus: LanPageStatus
    where TLanRouterStatus: LanRouterStatus
{ }