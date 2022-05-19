using System.Net;
using JsTypes;
using Router.Domain;
using Router.TpLink.Status.Lan.Network;

namespace Router.TpLink.TLWR741ND.Status.Lan.NetworkCfg;

public class TLWR741NDLanRouterStatusExtractor : LanNetworkRouterStatusExtractor<TLWR741NDLanNetworkPageStatus, TLWR741NDLanNetworkRouterStatus>
{
       private const int MAC = 0;
       private const int IP = 1;
       private const int Mask = 3;
    public override TLWR741NDLanNetworkRouterStatus ExtractStatus(TLWR741NDLanNetworkPageStatus status)
    {
        var lan = status.LanPara;
        var ip = IPAddress.Parse(( ( JsString ) lan[IP] ).Value);
        var mask = SubnetMask.Parse(( ( JsString ) lan[Mask] ).Value);
        var mac = MacAddress.Parse(( ( JsString ) lan[MAC] ).Value);
        return new TLWR741NDLanNetworkRouterStatus(ip, mask, mac);
    }
}