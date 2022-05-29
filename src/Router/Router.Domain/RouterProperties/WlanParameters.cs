namespace Router.Domain.RouterProperties;

public class WlanParameters
{
    public WlanParameters(string ssid, string password, bool isActive, Channel channel, Rate rate)
    {
        ArgumentNullException.ThrowIfNull(ssid);
        ArgumentNullException.ThrowIfNull(password);
        ArgumentNullException.ThrowIfNull(channel);
        ArgumentNullException.ThrowIfNull(rate);
        SSID = ssid;
        Password = password;
        Channel = channel;
        Rate = rate;
        IsActive = isActive;
    }

    public string SSID { get; }
    public string Password { get; }
    public bool IsActive { get; }
    public Channel Channel { get; }
    public Rate Rate { get; }
}