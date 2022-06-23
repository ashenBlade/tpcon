using System.Net;
using Router;
using Router.Domain;
using Router.Domain.Lan;

namespace Router.Commands.TpLink.TLWR741ND.Status.Lan.NetworkCfg;

public class LanNetworkRouterStatus : LanRouterStatus
{
    public LanNetworkRouterStatus(IPAddress ipAddress,
                                  SubnetMask currentSubnetMask,
                                  MacAddress macAddress,
                                  SubnetMask customSubnetMask)
    {
        ArgumentNullException.ThrowIfNull(ipAddress);
        ArgumentNullException.ThrowIfNull(macAddress);
        IpAddress = ipAddress;
        CurrentSubnetMask = currentSubnetMask;
        MacAddress = macAddress;
        CustomSubnetMask = customSubnetMask;
    }

    public IPAddress IpAddress { get; }
    public SubnetMask CurrentSubnetMask { get; }
    public MacAddress MacAddress { get; }
    public SubnetMask CustomSubnetMask { get; }

    public int SubnetMaskIndex => CurrentSubnetMask.MaskLength switch
                                  {
                                      8  => 0,
                                      16 => 1,
                                      24 => 2,
                                      _  => 3
                                  };

    public LanNetworkRouterStatus WithIpAddress(IPAddress address)
    {
        return new LanNetworkRouterStatus(address, CurrentSubnetMask, MacAddress, CustomSubnetMask);
    }

    public LanNetworkRouterStatus WithMacAddress(MacAddress macAddress)
    {
        return new LanNetworkRouterStatus(IpAddress, CurrentSubnetMask, macAddress, CustomSubnetMask);
    }

    public LanNetworkRouterStatus WithSubnetMask(SubnetMask mask)
    {
        return new LanNetworkRouterStatus(IpAddress, mask, MacAddress, mask);
    }
}