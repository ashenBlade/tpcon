using JsTypes;
using Router.Domain.Exceptions;
using Router.Domain.RouterProperties;
using Router.TpLink.TLWR741ND.Status;
using Router.TpLink.TLWR741ND.Status.Lan.NetworkCfg;

namespace Router.TpLink.TLWR741ND;

public class TLWR741NDTpLinkLanConfigurator : ILanConfigurator
{
    private readonly IRouterHttpMessageSender _messageSender;
    private readonly IRouterStatusExtractor<LanNetworkPageStatus, LanNetworkRouterStatus> _networkExtractor;

    public TLWR741NDTpLinkLanConfigurator(IRouterHttpMessageSender messageSender,
                                          IRouterStatusExtractor<LanNetworkPageStatus, LanNetworkRouterStatus> networkExtractor)
    {
        _messageSender = messageSender;
        _networkExtractor = networkExtractor;
    }
    
    public async Task<LanParameters> GetStatusAsync()
    {
        var variables = await _messageSender.SendMessageAndParseAsync("userRpm/NetworkCfgRpm.htm");
        var lanPara = variables.FirstOrDefault(v => v.Name == "lanPara")?.Value as JsArray 
                   ?? throw new InvalidRouterResponseException(); 
        var status = _networkExtractor.ExtractStatus(new LanNetworkPageStatus(lanPara));
        return new LanParameters(status.MacAddress, status.IpAddress, status.SubnetMask);
    }
}