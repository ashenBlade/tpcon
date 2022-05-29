using System.ComponentModel;

namespace Router.TpLink.TLWR741ND.Commands.DisplayStatus;

public class WlanDisplayStatus
{
    public WlanDisplayStatus(string password, string ssid, bool enabled, string channelNumber, string rate, string channelWidth)
    {
        Password = password;
        SSID = ssid;
        Enabled = enabled;
        ChannelNumber = channelNumber;
        Rate = rate;
        ChannelWidth = channelWidth;
    }
    
    [DisplayName("WPA/WPA2 password")]
    public string Password { get; }
    
    [DisplayName("SSID")]
    public string SSID { get; }
    
    [DisplayName("WiFi enabled")]
    public bool Enabled { get; }

    [DisplayName("Channel number")]
    public string ChannelNumber { get; }
    
    [DisplayName("Channel width")]
    public string ChannelWidth { get; }

    [DisplayName("Rate")]
    public string Rate { get; }
}