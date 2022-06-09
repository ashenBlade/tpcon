namespace Router.Domain.Wlan;

public class Channel
{
    public ChannelNumber Number { get; }
    public ChannelWidth Width { get; }

    public Channel(ChannelNumber number, ChannelWidth width)
    {
        Number = number;
        Width = width;
    }
}