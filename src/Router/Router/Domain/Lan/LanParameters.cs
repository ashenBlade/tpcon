using System.Net;

namespace Router.Domain.Lan;

public class LanParameters
{
    public LanParameters(MacAddress macAddress, IPAddress ipAddress, SubnetMask subnetMask)
    {
        MacAddress = macAddress ?? throw new ArgumentNullException(nameof(macAddress));
        IpAddress = ipAddress ?? throw new ArgumentNullException(nameof(ipAddress));
        SubnetMask = subnetMask;
    }
    
    public MacAddress MacAddress { get; }
    public IPAddress IpAddress { get; }
    public SubnetMask SubnetMask { get; }
}