using Router;
using Router.Domain;
using Router.Domain.Wlan;

namespace Router.Commands.TpLink.TLWR741ND.Status.Wlan.Network;

public class WlanNetworkRouterStatus : WlanRouterStatus
{
    public WlanNetworkRouterStatus(string ssid, bool enabled, Channel channel, Rate rate)
    {
        SSID = ssid;
        Enabled = enabled;
        Channel = channel;
        Rate = rate;
    }

    public string SSID { get; }
    public bool Enabled { get; }
    public Channel Channel { get; }
    public Rate Rate { get; }
}