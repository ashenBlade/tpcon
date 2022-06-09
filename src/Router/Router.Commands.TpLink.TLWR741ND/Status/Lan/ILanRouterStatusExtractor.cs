namespace Router.Commands.TpLink.TLWR741ND.Status.Lan;

public interface ILanRouterStatusExtractor<TLanPageStatus, TLanRouterStatus> 
    : IRouterStatusExtractor<TLanPageStatus, TLanRouterStatus>
    where TLanPageStatus: LanPageStatus
    where TLanRouterStatus: LanRouterStatus
{ }