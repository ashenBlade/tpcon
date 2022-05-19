using System.Net;
using Router.Domain;
using Router.TpLink.Status.Lan.Network;

namespace Router.TpLink.TLWR741ND.Status.Lan.NetworkCfg;

public class TLWR741NDLanNetworkRouterStatus : LanNetworkRouterStatus
{
    public TLWR741NDLanNetworkRouterStatus(IPAddress ipAddress, SubnetMask subnetMask, MacAddress macAddress)
    {
        IpAddress = ipAddress;
        SubnetMask = subnetMask;
        MacAddress = macAddress;
    }

    public IPAddress IpAddress { get; set; }
    public SubnetMask SubnetMask { get; set; }
    public MacAddress MacAddress { get; set; }
}