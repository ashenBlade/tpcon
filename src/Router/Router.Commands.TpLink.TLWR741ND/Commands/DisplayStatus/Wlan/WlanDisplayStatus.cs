using System.ComponentModel;
using Router.Domain.Wlan;

namespace Router.Commands.TpLink.TLWR741ND.Commands.DisplayStatus.Wlan;

public class WlanDisplayStatus : TpLink.Commands.DisplayStatus
{
    public WlanDisplayStatus(WlanParameters wlan)
        : this(wlan.SSID, wlan.IsActive, wlan.Channel, wlan.Rate, wlan.Security)
    { }
    
    public WlanDisplayStatus(string ssid,
                             bool enabled,
                             Channel channel,
                             Rate rate,
                             Security security)
    {
        SSID = ssid;
        Enabled = enabled;
        ChannelNumber = channel.Number.ToString();
        ChannelWidth = channel.Width.ToString();
        Rate = rate.ToString();
        Security = security.Name;
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