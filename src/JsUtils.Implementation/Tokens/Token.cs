namespace JsUtils.Implementation.Tokens;

public class Token: IEquatable<Token>
{
    #region Basic single tokens

    public static Token LeftParenthesis => new('(');
    public static Token RightParenthesis => new(')');
    public static Token LeftBrace => new('{');
    public static Token RightBrace => new('}');
    public static Token Less => new('<');
    public static Token Greater => new('>');
    public static Token Minus => new('-');
    public static Token Plus => new('+');
    public static Token Negate => new('!');
    public static Token Semicolon => new(';');
    public static Token Equal => new('=');

    #endregion
    public int Tag { get; }

    public Token(int tag)
    {
        Tag = tag;
    }

    public bool Equals(Token? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return Tag == other.Tag;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals(( Token ) obj);
    }

    public override int GetHashCode()
    {
        return Tag;
    }
}