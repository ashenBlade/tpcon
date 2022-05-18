using System.ComponentModel;
using System.Net;
using Router.Domain;

namespace Router.TpLink.Commands.DTO;

public class LanDisplayStatus
{
    public LanDisplayStatus(IPAddress routerAddress, MacAddress macAddress, SubnetMask subnetMask)
    {
        RouterAddress = routerAddress;
        MacAddress = macAddress;
        SubnetMask = subnetMask;
    }

    [DisplayName("Router address")]
    public IPAddress RouterAddress { get; }

    [DisplayName("Mac address")]
    public MacAddress MacAddress { get; }

    [DisplayName("Subnet mask")]
    public SubnetMask SubnetMask { get; }
}