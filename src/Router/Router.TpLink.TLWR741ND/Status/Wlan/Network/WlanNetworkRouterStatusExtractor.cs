using JsTypes;
using JsUtils.Implementation;
using Router.Domain;

namespace Router.TpLink.TLWR741ND.Status.Wlan.Network;

public class WlanNetworkRouterStatusExtractor
    : IWlanRouterStatusExtractor<WlanNetworkPageStatus, WlanNetworkRouterStatus>
{
    public const int SSID = 3;
    public const int Enabled = 8;
    public const int Rate = 12;
    public const int ChannelNumber = 10;
    public const int ChannelWidth = 11;
    
    
    private const int ChannelNumberAuto = 15;
    private const int ChannelWidthAuto = 2;
    
    public WlanNetworkRouterStatus ExtractStatus(WlanNetworkPageStatus status)
    {
        var wlan = status.WlanPara;
        var ssid = wlan[SSID].GetString();
        var enabled = wlan[Enabled].GetInt() == 1;
        var rate = GetRate(wlan, status.RateTable);
        var channel = GetChannel(wlan);
        return new WlanNetworkRouterStatus(ssid, enabled, channel, rate);
    }

    private static Channel GetChannel(JsArray wlanPara)
    {
        var (number, width) = ( wlanPara[ChannelNumber].GetInt(), wlanPara[ChannelWidth].GetInt() );
        return new Channel(number is ChannelNumberAuto ? Domain.ChannelNumber.Auto :  new ChannelNumber(number), 
                           width is ChannelWidthAuto ? Domain.ChannelWidth.Auto : new ChannelWidth(width));
    }
    
    private static Rate GetRate(JsArray wlanPara, JsArray rateTable)
    {
        // Rate table is array in form of 
        // [i] - speed string to display, [i + 1] - speed mask
        // i.e. var rateTable = new Array("1Mbps", 481, "2Mbps", 481)
        var speedString = rateTable[wlanPara[Rate].GetInt() - 1].GetString();
        var speed = int.Parse( speedString.Remove(speedString.IndexOf("Mbps", StringComparison.InvariantCulture)) );
        
        // For this router, only Mbps applied
        return new Rate(speed, NetworkSpeedMeasurement.Mbps);
    }
}