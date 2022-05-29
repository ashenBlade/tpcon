using Router.Domain;
using Router.Domain.RouterProperties;
using Router.TpLink.TLWR741ND.Commands.DisplayStatus;

namespace Router.TpLink.TLWR741ND.Utils;

public static class DisplayStatusExtensions
{
    public static WlanDisplayStatus ToDisplayStatus(this WlanParameters wlan)
    {
        return new WlanDisplayStatus(wlan.Password, wlan.SSID, wlan.IsActive, FormatChannelNumber(wlan.Channel.Number),
                                     FormatRate(wlan.Rate), FormatChannelWidth(wlan.Channel.Width));
    }

    private static string FormatChannelWidth(int channelWidth)
    {
        return channelWidth is 2
                   ? "Auto"
                   : $"{channelWidth}MHz";
    }

    private static string FormatRate(Rate rate) => $"{rate.Speed} {FormatNetworkSpeedMeasurement(rate.Measurement)}";

    private static string FormatNetworkSpeedMeasurement(NetworkSpeedMeasurement measurement) 
        => measurement switch
           {
               NetworkSpeedMeasurement .Kbps => "Kbps",
               NetworkSpeedMeasurement .Mbps => "Mbps",
               NetworkSpeedMeasurement .Gbps => "Gbps",
               _                             => "Unknown"
           };
    private static string FormatChannel(Channel channel) => $"{FormatChannelNumber(channel.Number)} {channel.Width}MHz";

    private static string FormatChannelNumber(int number) => number == 15
                                                                 ? "Auto"
                                                                 : number.ToString();
}