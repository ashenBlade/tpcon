using System.ComponentModel;

namespace Router.TpLink.TLWR741ND.Commands.DisplayStatus;

public class WlanDisplayStatus
{
    public WlanDisplayStatus(string password, string ssid, bool enabled)
    {
        Password = password;
        SSID = ssid;
        Enabled = enabled;
    }
    
    [DisplayName("WPA/WPA2 password")]
    public string Password { get; }
    [DisplayName("SSID")]
    public string SSID { get; }
    [DisplayName("WiFi enabled")]
    public bool Enabled { get; }
}