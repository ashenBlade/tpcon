namespace JsUtils.Implementation;

public class UnexpectedCharacterException : ParsingException
{
    public string Description { get; }
    public string Text { get; }
    public int Position { get; }
    public UnexpectedCharacterException(string text, int position, string description = "")
    {
        Text = text;
        Position = position;
        Description = description;
    }

    public override string ToString()
    {
        return Description == string.Empty
                   ? $"Unexpected character at position: {Position}"
                   : Description;
    }
}