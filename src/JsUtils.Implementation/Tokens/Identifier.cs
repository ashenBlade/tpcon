namespace JsUtils.Implementation.Tokens;

public class Identifier: Word, IEquatable<Identifier>
{
    public static Token Token => new(Tags.Id);
    public Identifier(string lexeme) : base(lexeme, Tags.Id) { }
    public bool Equals(Identifier? other)
    {
        return other is not null && other.Lexeme == Lexeme;
    }

    public override bool Equals(object obj)
    {
        return Equals(obj as Identifier);
    }
}