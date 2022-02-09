namespace JsUtils.Implementation;

public class UnexpectedTokenException : Exception
{
    public string Description { get; }
    public string Text { get; }
    public int Position { get; }
    public UnexpectedTokenException(string text, int position, string description)
    {
        Text = text;
        Position = position;
        Description = description;
    }
}