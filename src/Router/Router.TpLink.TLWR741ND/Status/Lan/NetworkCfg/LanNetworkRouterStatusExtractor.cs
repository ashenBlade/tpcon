using System.Net;
using JsTypes;
using Router.Domain;

namespace Router.TpLink.TLWR741ND.Status.Lan.NetworkCfg;

public class LanNetworkRouterStatusExtractor
    : ILanRouterStatusExtractor<LanNetworkPageStatus, LanNetworkRouterStatus>
{
    private const int MAC = 0;
    private const int IP = 1;
    private const int Mask = 3;
    
    public LanNetworkRouterStatus ExtractStatus(LanNetworkPageStatus status)
    {
        var lan = status.LanPara;
        var ip = IPAddress.Parse(( ( JsString ) lan[IP] ).Value);
        var mask = SubnetMask.Parse(( ( JsString ) lan[Mask] ).Value);
        var mac = MacAddress.Parse(( ( JsString ) lan[MAC] ).Value);
        return new LanNetworkRouterStatus(ip, mask, mac);
    }
}