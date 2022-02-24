using JsUtils.Implementation.Tokens;

namespace JsUtils.Implementation;

public class UnexpectedTokenException : ParsingException
{
    public string Description { get; }
    public int Expected { get; }
    public int Actual { get; }

    public UnexpectedTokenException(Token given, Token expected) : this(given.Tag, expected.Tag)
    {
        Expected = expected.Tag;
        Actual = given.Tag;
    }

    public UnexpectedTokenException(int actualTag, int expectedTag) :
        this($"Expected token tag: '{actualTag}'. Given tag: '{expectedTag}'")
    {
        Expected = expectedTag;
        Actual = actualTag;
    }

    public UnexpectedTokenException(string description)
    {
        Description = description;
    }


    public override string ToString()
    {
        return Description;
    }
}