using System.ComponentModel;
using System.Net;
using Router;
using Router.Domain;
using Router.Domain.Lan;

namespace Router.Commands.TpLink.TLWR741ND.Commands.DisplayStatus;

public class LanDisplayStatus : TpLink.Commands.DisplayStatus
{
    public LanDisplayStatus(LanParameters lan) 
        :this(lan.IpAddress, lan.MacAddress, lan.SubnetMask)
    { }
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