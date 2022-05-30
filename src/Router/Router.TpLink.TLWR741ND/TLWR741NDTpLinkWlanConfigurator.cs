using System.Web;
using JsTypes;
using Router.Domain.Infrastructure.Security;
using Router.Domain.RouterProperties;
using Router.TpLink.Exceptions;
using Router.TpLink.TLWR741ND.Status;
using Router.TpLink.TLWR741ND.Status.Wlan.Network;
using Router.TpLink.TLWR741ND.Status.Wlan.Security;

namespace Router.TpLink.TLWR741ND;

internal class TLWR741NDTpLinkWlanConfigurator : IWlanConfigurator
{
    private readonly IRouterHttpMessageSender _messageSender;
    private readonly IRouterStatusExtractor<WlanNetworkPageStatus, WlanNetworkRouterStatus> _networkExtractor;
    private readonly IRouterStatusExtractor<WlanSecurityPageStatus, WlanSecurityRouterStatus> _securityExtractor;

    public TLWR741NDTpLinkWlanConfigurator(IRouterHttpMessageSender messageSender, 
                                           IRouterStatusExtractor<WlanNetworkPageStatus, WlanNetworkRouterStatus> networkExtractor, 
                                           IRouterStatusExtractor<WlanSecurityPageStatus, WlanSecurityRouterStatus> securityExtractor)
    {
        _messageSender = messageSender;
        _networkExtractor = networkExtractor;
        _securityExtractor = securityExtractor;
    }
    public async Task<WlanParameters> GetStatusAsync()
    {
        var network = ( await _messageSender.SendMessageAndParseAsync(WlanNetworkPagePath) )
           .ToDictionary(v => v.Name, v => v.Value);
        var security = ( await _messageSender.SendMessageAndParseAsync(WlanSecurityPagePath) )
           .ToDictionary(v => v.Name, v => v.Value);
        var networkWlanPara = network["wlanPara"] 
                           ?? throw new MissingVariableInRouterResponseException("wlanPara", WlanNetworkPagePath);
        var rateTable = network["rateTable"]
                     ?? throw new MissingVariableInRouterResponseException("rateTable", WlanNetworkPagePath);
        var securityWlanPara = security["wlanPara"]
                            ?? throw new MissingVariableInRouterResponseException("wlanPara", WlanSecurityPagePath);

        var networkPageStatus = new WlanNetworkPageStatus(networkWlanPara as JsArray
                                                       ?? throw new ExpectedVariableTypeMismatchException("wlanPara", typeof(JsArray), networkWlanPara.GetType()),
                                                          rateTable as JsArray
                                                       ?? throw new ExpectedVariableTypeMismatchException("rateTable", typeof(JsArray), rateTable.GetType()));
        var securityPageStatus = new WlanSecurityPageStatus(securityWlanPara as JsArray
                                                         ?? throw new ExpectedVariableTypeMismatchException("wlanPara", typeof(JsArray), securityWlanPara.GetType()));
        
        var networkStatus = _networkExtractor.ExtractStatus(networkPageStatus);
        var securityStatus = _securityExtractor.ExtractStatus(securityPageStatus);
        return new WlanParameters(networkStatus.SSID, 
                                  networkStatus.Enabled, 
                                  networkStatus.Channel, 
                                  networkStatus.Rate,
                                  new NoneSecurity());
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