namespace JsUtils.Implementation;

public class UnexpectedCharacterException : ParsingException
{
    public string Description { get; }
    public string Text { get; }
    public int Position { get; }
    public string Expected { get; }

    public UnexpectedCharacterException(string text, int position, char expected, string? description = null)
    : this(text, position, expected.ToString(), description)
    { }

    public UnexpectedCharacterException(string text, int position, string expected, string? description = null)
    {
        Text = text;
        Position = position;
        Expected = expected;
        Description = description ?? $"Expected: {expected}\nGiven: {text[position]}";
    }

    public override string ToString()
    {
        return Description == string.Empty
                   ? $"Unexpected character at position: {Position}"
                   : Description;
    }
}