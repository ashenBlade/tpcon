namespace JsUtils.Implementation.Tokens;

public class Identifier: Word, IEquatable<Identifier>
{
    public Identifier(string lexeme) : base(lexeme, Tags.Id) { }
    public bool Equals(Identifier? other)
    {
        return other is not null && other.Lexeme == Lexeme;
    }
}