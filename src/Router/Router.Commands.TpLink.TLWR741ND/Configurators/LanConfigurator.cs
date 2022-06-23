using System.Net;
using JsTypes;
using Router.Commands.TpLink.Configurators.Lan;
using Router.Commands.TpLink.TLWR741ND.Status;
using Router.Commands.TpLink.TLWR741ND.Status.Lan.NetworkCfg;
using Router.Domain.Lan;

namespace Router.Commands.TpLink.TLWR741ND.Configurators;

public class LanConfigurator : BaseLanConfigurator
{
    private readonly IRouterStatusExtractor<LanNetworkPageStatus, LanNetworkRouterStatus> _networkExtractor;

    public LanConfigurator(IRouterHttpMessageSender messageSender,
                           IRouterStatusExtractor<LanNetworkPageStatus, LanNetworkRouterStatus> networkExtractor)
        : base(messageSender)
    {
        _networkExtractor = networkExtractor;
    }

    private const string NetworkCfgPath = "userRpm/NetworkCfgRpm.htm";

    public override async Task<LanParameters> GetStatusAsync()
    {
        var status = await GetLanNetworkRouterStatusAsync();
        return new LanParameters(status.MacAddress,
                                 status.IpAddress,
                                 status.CurrentSubnetMask);
    }

    private async Task<LanNetworkRouterStatus> GetLanNetworkRouterStatusAsync()
    {
        var lan = await GetNetworkCfgPageStatusAsync();
        var lanPara = GetRequiredArray(lan, "lanPara", NetworkCfgPath);
        return _networkExtractor
           .ExtractStatus(new LanNetworkPageStatus(lanPara));
    }

    private Task<Dictionary<string, JsType>> GetNetworkCfgPageStatusAsync()
    {
        return GetPageVariablesAsync("userRpm/NetworkCfgRpm.htm");
    }

    public override async Task SetIpAsync(IPAddress address)
    {
        var status = ( await GetLanNetworkRouterStatusAsync() ).WithIpAddress(address);
        await SetStatusAsync(status);
    }

    public override async Task SetSubnetMaskAsync(SubnetMask mask)
    {
        var status = ( await GetLanNetworkRouterStatusAsync() ).WithSubnetMask(mask);
        await SetStatusAsync(status);
    }

    private async Task SetStatusAsync(LanNetworkRouterStatus lan)
    {
        var parameters = new KeyValuePair<string, string>[]
                         {
                             new("lanip", lan.IpAddress.ToString()), new("lanmask", lan.SubnetMaskIndex.ToString()),
                             new("inputMask", lan.CustomSubnetMask.ToString()),
                         };

        await MessageSender.SendMessageAndSaveAsync(NetworkCfgPath, parameters);
    }
}