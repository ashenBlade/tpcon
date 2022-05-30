using Router.Domain;
using Router.Domain.RouterProperties;
using Router.TpLink.TLWR741ND.Commands.DisplayStatus;

namespace Router.TpLink.TLWR741ND.Utils;

public static class DisplayStatusExtensions
{
    public static WlanDisplayStatus ToDisplayStatus(this WlanParameters wlan)
    {
        return new WlanDisplayStatus(string.Empty, wlan.SSID, wlan.IsActive, wlan.Channel.Number.ToString(),
                                     FormatRate(wlan.Rate), wlan.Channel.Width.ToString());
    }

    private static string FormatRate(Rate rate) => $"{rate.Speed} {FormatNetworkSpeedMeasurement(rate.Measurement)}";

    private static string FormatNetworkSpeedMeasurement(NetworkSpeedMeasurement measurement) 
        => measurement switch
           {
               NetworkSpeedMeasurement.Kbps => "Kbps",
               NetworkSpeedMeasurement.Mbps => "Mbps",
               NetworkSpeedMeasurement.Gbps => "Gbps",
               _                             => "Unknown"
           };
}