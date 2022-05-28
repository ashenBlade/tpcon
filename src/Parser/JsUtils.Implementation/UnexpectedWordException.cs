using JsUtils.Implementation.Tokens;

namespace JsUtils.Implementation;

public class UnexpectedWordException : UnexpectedTokenException
{
    public new Word Actual { get; }
    public new Word Expected { get; }
    public UnexpectedWordException(Word given, Word expected) 
        : this($"Expected word: \"{expected.Lexeme}\", but given: \"{given.Lexeme}\"", expected, given)
    { }

    public UnexpectedWordException(string description, Word expected, Word actual) : base(description, expected, actual)
    {
        Actual = actual;
        Expected = expected;
    }
}