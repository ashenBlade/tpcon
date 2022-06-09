namespace Router.Domain.Wlan;

public readonly struct ChannelWidth
{
    public static ChannelWidth Auto = new(AutoChannel);
    public string Width { get; } = string.Empty;

    public ChannelWidth(int width)
        : this(width < 1 
                   ? throw new ArgumentOutOfRangeException(nameof(width), "Channel width must be positive") 
                   : width.ToString())
    { }

    private const string AutoChannel = "Auto";
    private ChannelWidth(string width)
    {
        Width = width;
    }

    public override string ToString()
    {
        return Width;
    }
}