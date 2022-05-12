using System.Net;
using System.Web;
using JsTypes;
using Router.Domain.RouterProperties;

namespace Router.TpLink.TLWR741ND;

internal class TLWR741NDTpLinkWlanConfigurator : IWlanConfigurator
{
    private readonly IRouterHttpMessageSender _messageSender;

    public TLWR741NDTpLinkWlanConfigurator(IRouterHttpMessageSender messageSender)
    {
        _messageSender = messageSender;
    }
    public async Task<WlanParameters> GetStatusAsync()
    {
        var wlanParametersArray =
            ( await _messageSender.SendMessageAndParseAsync("userRpm/StatusRpm.htm") )
           .First(v => v.Name is "wlanPara").Value as JsArray;
        var isActive = ( wlanParametersArray![0] as JsNumber )!.Value == 1;
        var ssid = ( wlanParametersArray[1] as JsString )!.Value;
        var ip = IPAddress.Parse(( wlanParametersArray[5] as JsString )!.Value);
        var password = ((( await _messageSender.SendMessageAndParseAsync("userRpm/WlanSecurityRpm.htm") )
                        .First(v => v.Name is "wlanPara")
                        .Value as JsArray)!
                        [9] as JsString)!.Value;
        return new WlanParameters(ssid, password, isActive, ip);
    }

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