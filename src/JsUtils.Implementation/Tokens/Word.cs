namespace JsUtils.Implementation.Tokens;

public class Word: Token, IEquatable<Word>
{
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