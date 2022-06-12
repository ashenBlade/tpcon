using Router.Commands.TpLink.TLWR741ND.Status.Wlan;
using Router.Commands.TpLink.TLWR741ND.Status.Wlan.Network;
using Router.Domain.Wlan;

namespace Router.Commands.TpLink.TLWR741ND.Tests.Mocks;

public class
    MockWlanNetworkRouterStatusExtractor : IWlanRouterStatusExtractor<WlanNetworkPageStatus, WlanNetworkRouterStatus>
{
    public bool Enabled { get; }
    public Channel Channel { get; }
    public string SSID { get; }
    public Rate Rate { get; }

    public MockWlanNetworkRouterStatusExtractor(string? ssid = null,
                                                bool enabled = true,
                                                ChannelNumber? number = null,
                                                ChannelWidth? width = null,
                                                int rate = 150,
                                                NetworkSpeedMeasurement measurement = NetworkSpeedMeasurement.Mbps)
    {
        SSID = ssid ?? string.Empty;
        Enabled = enabled;
        Channel = new Channel(number ?? ChannelNumber.Auto, width ?? ChannelWidth.Auto);
        Rate = new Rate(rate, measurement);
    }

    public WlanNetworkRouterStatus ExtractStatus(WlanNetworkPageStatus status)
    {
        return new WlanNetworkRouterStatus(SSID, Enabled, Channel, Rate);
    }
}