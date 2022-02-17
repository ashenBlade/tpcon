namespace JsUtils.Implementation.Tokens;

public class Token: IEquatable<Token>
{
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