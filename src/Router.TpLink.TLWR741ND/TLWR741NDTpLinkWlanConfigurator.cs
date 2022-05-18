using System.Net;
using System.Web;
using JsTypes;
using Router.Domain.Exceptions;
using Router.Domain.RouterProperties;
using Router.TpLink.Exceptions;
using Router.TpLink.Status;
using Router.TpLink.Status.Wlan.Network;
using Router.TpLink.Status.Wlan.Security;
using Router.TpLink.TLWR741ND.Status.Wlan;
using Router.TpLink.TLWR741ND.Status.Wlan.Security;

namespace Router.TpLink.TLWR741ND;

internal class TLWR741NDTpLinkWlanConfigurator : IWlanConfigurator
{
    private readonly IRouterHttpMessageSender _messageSender;
    private readonly IRouterStatusExtractor<TLWR741NDWlanNetworkPageStatus, TLWR741NDWlanNetworkRouterStatus> _networkExtractor;
    private readonly IRouterStatusExtractor<TLWR741NDWlanSecurityPageStatus, TLWR741NDWlanSecurityRouterStatus> _securityExtractor;

    public TLWR741NDTpLinkWlanConfigurator(IRouterHttpMessageSender messageSender, 
                                           IRouterStatusExtractor<TLWR741NDWlanNetworkPageStatus, TLWR741NDWlanNetworkRouterStatus> networkExtractor, 
                                           IRouterStatusExtractor<TLWR741NDWlanSecurityPageStatus, TLWR741NDWlanSecurityRouterStatus> securityExtractor)
    {
        _messageSender = messageSender;
        _networkExtractor = networkExtractor;
        _securityExtractor = securityExtractor;
    }
    public async Task<WlanParameters> GetStatusAsync()
    {
        var network = await _messageSender.SendMessageAndParseAsync(WlanNetworkPagePath);
        var security = await _messageSender.SendMessageAndParseAsync(WlanSecurityPagePath);
        var networkWlanPara = network.FirstOrDefault(v => v.Name == "wlanPara")?.Value! 
                           ?? throw new MissingVariableInRouterResponseException("wlanPara", WlanNetworkPagePath);

        var securityWlanPara = security.FirstOrDefault(v => v.Name == "wlanPara")?.Value
                            ?? throw new MissingVariableInRouterResponseException("wlanPara", WlanSecurityPagePath);

        var networkPageStatus = new TLWR741NDWlanNetworkPageStatus(networkWlanPara as JsArray
                                                                ?? throw new ExpectedVariableTypeMismatchException("wlanPara", typeof(JsArray), networkWlanPara.GetType()));
        var securityPageStatus = new TLWR741NDWlanSecurityPageStatus(securityWlanPara as JsArray
                                                                  ?? throw new ExpectedVariableTypeMismatchException("wlanPara", typeof(JsArray), securityWlanPara.GetType()));
        
        var networkStatus = _networkExtractor.ExtractStatus(networkPageStatus);
        var securityStatus = _securityExtractor.ExtractStatus(securityPageStatus);
        return new WlanParameters(networkStatus.SSID, securityStatus.Password, networkStatus.Enabled);
    }

    private static string WlanNetworkPagePath =>
        "userRpm/WlanNetworkRpm.htm";

    private static string WlanSecurityPagePath =>
        "userRpm/WlanSecurityRpm.htm";

    private Task SetWirelessRadioStatusInternal(bool active)
    {
        return _messageSender.SendMessageAsync(new RouterHttpMessage("userRpm/WlanNetworkRpm.htm",
                                                                     new KeyValuePair<string, string>[]
                                                                     {
                                                                         new("ap", active
                                                                                       ? "1"
                                                                                       : "0"),
                                                                         new("Save", "Save")
                                                                     }));
    }
    
    public Task EnableAsync()
    {
        return SetWirelessRadioStatusInternal(true);
    }

    public Task DisableAsync()
    {
        return SetWirelessRadioStatusInternal(false);
    }

    public Task SetPasswordAsync(string password)
    {
        if (string.IsNullOrEmpty(password))
        {
            throw new ArgumentException("Password can not be null");
        }
        return _messageSender.SendMessageAsync(new RouterHttpMessage("userRpm/WlanSecurityRpm.htm",
                                                                     new KeyValuePair<string, string>[]
                                                                     {
                                                                         new("pskSecret", HttpUtility.UrlEncode(password)), 
                                                                         new("Save", "Save")
                                                                     }));
    }

    public Task SetSsidAsync(string ssid)
    {
        if (string.IsNullOrEmpty(ssid))
        {
            throw new ArgumentOutOfRangeException(ssid);
        }

        return _messageSender.SendMessageAsync(new RouterHttpMessage("userRpm/WlanNetworkRpm.htm",
                                                                     new KeyValuePair<string, string>[]
                                                                     {
                                                                         new("ap", "1"),
                                                                         new("ssid1", HttpUtility.UrlEncode(ssid)),
                                                                         new("broadcast", "2"), 
                                                                         new("Save", "Save")
                                                                     }));
    }
}