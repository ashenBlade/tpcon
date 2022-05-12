using System.Net;
using System.Web;
using JsTypes;
using Router.Domain.RouterProperties;

namespace Router.TpLink.TLWR741ND;

public class TLWR741NDTpLinkRouter : TpLinkRouter
{
    public TLWR741NDTpLinkRouter(IRouterHttpMessageSender sender) 
        : base(sender) 
    { }
    
    public override async Task<WlanParameters> GetWlanParametersAsync()
    {
        var wlanParametersArray =
            ( await MessageSender.SendMessageAndParseAsync("userRpm/StatusRpm.htm") )
           .First(v => v.Name is "wlanPara").Value as JsArray;
        var isActive = ( wlanParametersArray![0] as JsNumber )!.Value == 1;
        var ssid = ( wlanParametersArray[1] as JsString )!.Value;
        var ip = IPAddress.Parse(( wlanParametersArray[5] as JsString )!.Value);
        var password = ((( await MessageSender.SendMessageAndParseAsync("userRpm/WlanSecurityRpm.htm") )
                        .First(v => v.Name is "wlanPara")
                        .Value as JsArray)!
                        [9] as JsString)!.Value;
        return new WlanParameters(ssid, password, isActive, ip);
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
    
    public override Task EnableWirelessRadioAsync()
    {
        return SetWirelessRadioStatusInternal(true);
    }

    public override Task DisableWirelessRadioAsync()
    {
        return SetWirelessRadioStatusInternal(false);
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
                                                                        new("broadcast", "2"), new("Save", "Save")
                                                                    }));
    }

    public override Task SetPasswordAsync(string password)
    {
        if (string.IsNullOrEmpty(password))
        {
            throw new ArgumentException(password);
        }

        if (password.Length < 8)
        {
            throw new ArgumentOutOfRangeException(password, "Password length must be greater than 8");
        }
        
        return MessageSender.SendMessageAsync(new RouterHttpMessage("userRpm/WlanSecurityRpm.htm",
                                                                    new KeyValuePair<string, string>[]
                                                                    {
                                                                        new("pskSecret", HttpUtility.UrlEncode(password)), 
                                                                        new("Save", "Save")
                                                                    }));
    }
    
}