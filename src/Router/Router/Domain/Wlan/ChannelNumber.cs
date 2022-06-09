namespace Router.Domain.Wlan;

public readonly struct ChannelNumber
{
    public static ChannelNumber Auto => new(AutoNumber);
    public string Number { get; } = string.Empty;

    public ChannelNumber(int number)
    : this(number < 1 
               ? throw new ArgumentOutOfRangeException(nameof(number), "Channel number must be positive") 
               : number.ToString())
    { }

    private ChannelNumber(string number)
    {
        Number = number;
    }

    private const string AutoNumber = "Auto";

    public override string ToString()
    {
        return Number;
    }
}