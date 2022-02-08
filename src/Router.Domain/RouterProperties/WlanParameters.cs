namespace Router.Domain.RouterProperties;

public struct WlanParameters
{
    public WlanParameters(string ssid, string password, bool isActive)
    {
        SSID = ssid;
        Password = password;
        IsActive = isActive;
    }
    public string SSID { get;  }
    public string Password { get; }
    public bool IsActive { get; }
}