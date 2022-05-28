namespace JsUtils.Implementation.Tokens;

public class Token: IEquatable<Token>
{
    #region Basic single tokens

    public static Token LeftParenthesis => new('(');
    public static Token RightParenthesis => new(')');
    public static Token LeftBrace => new('{');
    public static Token RightBrace => new('}');
    public static Token LeftBracket => new('[');
    public static Token RightBracket => new(']');
    public static Token Comma => new(',');
    public static Token Dot => new('.');
    public static Token Less => new('<');
    public static Token Greater => new('>');
    public static Token Minus => new('-');
    public static Token Plus => new('+');
    public static Token Negate => new('!');
    public static Token Semicolon => new(';');
    public static Token Equal => new('=');
    public static Token BitwiseOr => new('|');
    public static Token BitwiseAnd => new('&');
    public static Token Xor => new('^');

    #endregion

    public static Token Undefined => new(Tags.Undefined);
    public static Token Null => new(Tags.Null);
    public static Token Comment => new(Tags.Comment);
    
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