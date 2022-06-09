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
    
    private static string NetworkCfgPath =>
        "userRpm/NetworkCfgRpm.htm";
    
    public override async Task<LanParameters> GetStatusAsync()
    {
        var lan = await GetPageVariablesAsync("userRpm/NetworkCfgRpm.htm");
        var lanPara = GetRequiredArray(lan, "lanPara", NetworkCfgPath);
        var status = _networkExtractor
           .ExtractStatus(new LanNetworkPageStatus(lanPara));
        return new LanParameters(status.MacAddress, 
                                 status.IpAddress, 
                                 status.SubnetMask);
    }
}