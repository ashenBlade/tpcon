namespace JsUtils.Implementation.Tokens;

public class Word: Token, IEquatable<Word>
{
    #region Basic words

    public static Word And => new("&&", Tags.And);
    public static Word Or => new("||", Tags.Or);
    public static Word Increment => new("++", Tags.Increment);
    public static Word Decrement => new("--", Tags.Decrement);
    public static Word Equality => new("==", Tags.Equality);
    public static Word StrongEquality => new("===", Tags.StrongEquality);
    
    #endregion
    public string Lexeme { get; }

    public Word(string lexeme, int tag): base(tag)
    {
        Lexeme = lexeme;
    }

    public bool Equals(Word? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return base.Equals(other)
            && Lexeme == other.Lexeme;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals(( Word ) obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(base.GetHashCode(), Lexeme);
    }
}