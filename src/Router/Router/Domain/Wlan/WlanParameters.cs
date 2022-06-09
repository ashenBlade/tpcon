namespace Router.Domain.Wlan;

public class WlanParameters
{
    public WlanParameters(string ssid, bool isActive, Channel channel, Rate rate, Security security)
    {
        ArgumentNullException.ThrowIfNull(ssid);
        ArgumentNullException.ThrowIfNull(security);
        ArgumentNullException.ThrowIfNull(channel);
        ArgumentNullException.ThrowIfNull(rate);
        SSID = ssid;
        Channel = channel;
        Rate = rate;
        Security = security;
        IsActive = isActive;
    }

    public string SSID { get; }
    public Security Security { get; }
    public bool IsActive { get; }
    public Channel Channel { get; }
    public Rate Rate { get; }
}