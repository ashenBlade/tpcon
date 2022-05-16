using JsUtils.Implementation.Tokens;

namespace JsUtils.Implementation;

public class UnexpectedTokenException : ParsingException
{
    public string Description { get; }
    public Token Expected { get; }
    public Token Actual { get; }

    public UnexpectedTokenException(Token expected, Token actual) : this($"Expected: {expected.Tag}\nGiven: {actual.Tag}", actual, expected)
    { }
    
    public UnexpectedTokenException(string description, Token expected, Token actual)
    {
        Description = description ?? string.Empty;
        Expected = expected;
        Actual = actual;
    }


    public override string ToString()
    {
        return Description;
    }
}