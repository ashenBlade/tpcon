using System.Net;

namespace Router.Domain.RouterProperties;

public struct LanParameters
{
    public LanParameters(MacAddress macAddress, IPAddress ipAddress, IPAddress subnetSubnetMask)
    {
        MacAddress = macAddress;
        IpAddress = ipAddress;
        SubnetMask = subnetSubnetMask;
    }
    
    public MacAddress MacAddress { get; }
    public IPAddress IpAddress { get; }
    public IPAddress SubnetMask { get; }
}