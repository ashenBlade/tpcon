using System.Net;
using Router.Domain;

namespace Router.TpLink.TLWR741ND.Status.Lan.NetworkCfg;

public class LanNetworkRouterStatus : LanRouterStatus
{
    public LanNetworkRouterStatus(IPAddress ipAddress, SubnetMask subnetMask, MacAddress macAddress)
    {
        ArgumentNullException.ThrowIfNull(ipAddress);
        ArgumentNullException.ThrowIfNull(macAddress);
        IpAddress = ipAddress;
        SubnetMask = subnetMask;
        MacAddress = macAddress;
    }

    public IPAddress IpAddress { get; }
    public SubnetMask SubnetMask { get; }
    public MacAddress MacAddress { get; }
}