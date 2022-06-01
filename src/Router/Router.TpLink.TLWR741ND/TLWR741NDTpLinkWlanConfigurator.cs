using System.Web;
using JsTypes;
using JsUtils.Implementation;
using Router.Domain.Exceptions;
using Router.Domain.Infrastructure.Security;
using Router.Domain.RouterProperties;
using Router.TpLink.Exceptions;
using Router.TpLink.TLWR741ND.Status;
using Router.TpLink.TLWR741ND.Status.Wlan.Network;
using Router.TpLink.TLWR741ND.Status.Wlan.Security;
using Router.TpLink.TLWR741ND.Utils;

namespace Router.TpLink.TLWR741ND;

internal class TLWR741NDTpLinkWlanConfigurator : BaseWlanConfigurator
{
    private static string WlanNetworkPagePath =>
        "userRpm/WlanNetworkRpm.htm";
    private static string WlanSecurityPagePath =>
        "userRpm/WlanSecurityRpm.htm";
    
    private readonly IRouterStatusExtractor<WlanNetworkPageStatus, WlanNetworkRouterStatus> _networkExtractor;
    private readonly IRouterStatusExtractor<WlanSecurityPageStatus, WlanSecurityRouterStatus> _securityExtractor;

    public TLWR741NDTpLinkWlanConfigurator(IRouterHttpMessageSender messageSender, 
                                           IRouterStatusExtractor<WlanNetworkPageStatus, WlanNetworkRouterStatus> networkExtractor, 
                                           IRouterStatusExtractor<WlanSecurityPageStatus, WlanSecurityRouterStatus> securityExtractor)
        : base(messageSender)
    {
        _networkExtractor = networkExtractor;
        _securityExtractor = securityExtractor;
    }
    public override async Task<WlanParameters> GetStatusAsync()
    {
        try
        {
            var network = await GetPageVariablesAsync(WlanNetworkPagePath);
            var security = await GetPageVariablesAsync(WlanSecurityPagePath);
        
            var networkWlanPara = GetRequiredArray(network, "wlanPara", WlanNetworkPagePath);
            var rateTable = GetRequiredArray(network, "rateTable", WlanNetworkPagePath);
            var securityWlanPara = GetRequiredArray(security, "wlanPara", WlanSecurityPagePath);
            var securityWlanList = GetRequiredArray(security, "wlanList", WlanSecurityPagePath);

            var networkStatus = _networkExtractor
               .ExtractStatus(new WlanNetworkPageStatus(networkWlanPara, rateTable));
            var securityStatus = _securityExtractor
               .ExtractStatus(new WlanSecurityPageStatus(securityWlanPara, securityWlanList));
            return new WlanParameters(networkStatus.SSID, 
                                      networkStatus.Enabled, 
                                      networkStatus.Channel, 
                                      networkStatus.Rate,
                                      securityStatus.Security);
        }
        catch (ArgumentOutOfRangeException)
        {
            throw new InvalidRouterResponseException();
        }
    }
    

    private Task SetWirelessRadioStatusInternal(bool active)
    {
        return MessageSender.SendMessageAsync(new RouterHttpMessage("userRpm/WlanNetworkRpm.htm",
                                                                     new KeyValuePair<string, string>[]
                                                                     {
                                                                         new("ap", active
                                                                                       ? "1"
                                                                                       : "0"),
                                                                         new("Save", "Save")
                                                                     }));
    }
    
    public override Task EnableAsync()
    {
        return SetWirelessRadioStatusInternal(true);
    }

    public override Task DisableAsync()
    {
        return SetWirelessRadioStatusInternal(false);
    }

    public override Task SetPasswordAsync(string password)
    {
        if (string.IsNullOrEmpty(password))
        {
            throw new ArgumentException("Password can not be null");
        }
        return MessageSender.SendMessageAsync(new RouterHttpMessage("userRpm/WlanSecurityRpm.htm",
                                                                     new KeyValuePair<string, string>[]
                                                                     {
                                                                         new("pskSecret", HttpUtility.UrlEncode(password)), 
                                                                         new("Save", "Save")
                                                                     }));
    }

    public override Task SetSsidAsync(string ssid)
    {
        if (string.IsNullOrEmpty(ssid))
        {
            throw new ArgumentOutOfRangeException(ssid);
        }

        return MessageSender.SendMessageAsync(new RouterHttpMessage("userRpm/WlanNetworkRpm.htm",
                                                                     new KeyValuePair<string, string>[]
                                                                     {
                                                                         new("ap", "1"),
                                                                         new("ssid1", HttpUtility.UrlEncode(ssid)),
                                                                         new("broadcast", "2"), 
                                                                         new("Save", "Save")
                                                                     }));
    }
}