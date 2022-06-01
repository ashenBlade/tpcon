using System.ComponentModel;

namespace Router.TpLink.TLWR741ND.Commands.DisplayStatus;

public class WlanDisplayStatus : BaseDisplayStatus
{
    public WlanDisplayStatus(string ssid, bool enabled, string channelNumber, string rate, string channelWidth, string security)
    {
        SSID = ssid;
        Enabled = enabled;
        ChannelNumber = channelNumber;
        Rate = rate;
        ChannelWidth = channelWidth;
        Security = security;
    }
    
    [DisplayName("SSID")]
    public string SSID { get; }
    
    [DisplayName("WiFi enabled")]
    public bool Enabled { get; }

    [DisplayName("Channel number")]
    public string ChannelNumber { get; }
    
    [DisplayName("Channel width")]
    public string ChannelWidth { get; }

    [DisplayName("Security")]
    public string Security { get; }

    [DisplayName("Rate")]
    public string Rate { get; }
}