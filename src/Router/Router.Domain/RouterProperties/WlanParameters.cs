namespace Router.Domain.RouterProperties;

public class WlanParameters
{
    public WlanParameters(string ssid, string password, bool isActive)
    {
        SSID = ssid ?? throw new ArgumentNullException(nameof(ssid));
        Password = password ?? throw new ArgumentNullException(nameof(password));
        IsActive = isActive;
    }

    public string SSID { get; }
    public string Password { get; }
    public bool IsActive { get; }
}