namespace Router.Commands.TpLink.TLWR741ND.Status;

public interface IRouterStatusExtractor<in TPageStatus, out TRouterStatus> where TPageStatus : PageStatus 
                                                                           where TRouterStatus: RouterStatus
{
    TRouterStatus ExtractStatus(TPageStatus status);
}