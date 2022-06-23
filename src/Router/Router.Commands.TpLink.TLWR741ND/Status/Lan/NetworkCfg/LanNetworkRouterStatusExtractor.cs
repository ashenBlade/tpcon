using System.Net;
using JsTypes;
using JsUtils.Implementation;
using Router;
using Router.Commands.TpLink.Exceptions;
using Router.Domain;
using Router.Domain.Lan;

namespace Router.Commands.TpLink.TLWR741ND.Status.Lan.NetworkCfg;

public class LanNetworkRouterStatusExtractor
    : ILanRouterStatusExtractor<LanNetworkPageStatus, LanNetworkRouterStatus>
{
    private const int MAC = 0;
    private const int IP = 1;
    private const int Mask = 2;
    private const int CustomMask = 3;

    public LanNetworkRouterStatus ExtractStatus(LanNetworkPageStatus status)
    {
        var lan = status.LanPara;
        var ip = IPAddress.Parse(lan[IP].GetString());
        var customMask = SubnetMask.Parse(lan[CustomMask].GetString());
        var mask = lan[Mask].GetInt() switch
                   {
                       0 => new SubnetMask(8),
                       1 => new SubnetMask(16),
                       2 => new SubnetMask(24),
                       3 => customMask,
                       _ => throw new ArgumentOutOfRangeRouterResponseException("mask", lan[Mask].ToString()!,
                                                                                "in range 0 - 3")
                   };
        var mac = MacAddress.Parse(lan[MAC].GetString());
        return new LanNetworkRouterStatus(ip, mask, mac, customMask);
    }
}