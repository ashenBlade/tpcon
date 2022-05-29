namespace Router.Domain;

public class Channel
{
    public Channel(int number, int width)
    {
        if (number < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(number), "Channel number can not be negative");
        }

        if (width < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(width), "Channel width can not be negative");
        }

        Width = width;
        Number = number;
    }
    public int Number { get; }
    /// <summary>
    /// Channel width in MHz
    /// </summary>
    public int Width { get; }

    public override string ToString()
    {
        return $"{Number} {Width}MHz";
    }
}