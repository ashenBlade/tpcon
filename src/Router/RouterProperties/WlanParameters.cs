using System.Net;

namespace Router.Domain.RouterProperties;

public class WlanParameters
{
    public WlanParameters(string ssid, string password, bool isActive, IPAddress routerAddress)
    {
        SSID = ssid ?? throw new ArgumentNullException(nameof(ssid));
        Password = password ?? throw new ArgumentNullException(nameof(password));
        IsActive = isActive;
        RouterAddress = routerAddress;
    }

    public string SSID { get; }
    public string Password { get; }
    public bool IsActive { get; }
    public IPAddress RouterAddress { get; }

    public WlanParameters WithPassword(string newPassword)
    {
        return new WlanParameters(SSID, newPassword ?? throw new ArgumentNullException(nameof(newPassword)), IsActive, RouterAddress);
    }

    public WlanParameters WithSSID(string newSSID)
    {
        return new WlanParameters(newSSID ?? throw new ArgumentNullException(nameof(newSSID)), Password, IsActive, RouterAddress);
    }

    public WlanParameters WithActiveState(bool isActive)
    {
        return new WlanParameters(SSID, Password, isActive, RouterAddress);
    }
}