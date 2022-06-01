using System.ComponentModel;
using System.Net;
using Router.Domain;

namespace Router.TpLink.TLWR741ND.Commands.DisplayStatus;

public class LanDisplayStatus : BaseDisplayStatus
{
    public LanDisplayStatus(IPAddress routerAddress, MacAddress macAddress, SubnetMask subnetMask)
    {
        RouterAddress = routerAddress.ToString();
        MacAddress = macAddress.ToString();
        SubnetMask = subnetMask.ToString();
    }

    [DisplayName("Router address")]
    public string RouterAddress { get; }

    [DisplayName("Mac address")]
    public string MacAddress { get; }

    [DisplayName("Subnet mask")]
    public string SubnetMask { get; }
}