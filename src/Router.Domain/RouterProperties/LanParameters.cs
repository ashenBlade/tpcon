using System.Net;

namespace Router.Domain.RouterProperties;

public struct LanParameters
{
    public LanParameters(MacAddress macAddress, IPAddress ipAddress, SubnetMask subnetMask)
    {
        MacAddress = macAddress;
        IpAddress = ipAddress;
        SubnetMask = subnetMask;
    }
    
    public MacAddress MacAddress { get; }
    public IPAddress IpAddress { get; }
    public SubnetMask SubnetMask { get; }
}