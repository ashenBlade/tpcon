namespace JsUtils.Implementation.Tokens;

public abstract class Literal : Token, IEquatable<Literal>
{
    public bool Equals(Literal? other)
    {
        return base.Equals(other);
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals(( Literal ) obj);
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
    

    protected Literal(int tag) : base(tag)
    { }
}

public class Literal<T>: Literal, IEquatable<T>
{
    public T Value { get; }

    public Literal(T value, int tag) : base(tag)
    {
        Value = value;
    }

    protected bool Equals(Literal<T> other)
    {
        return base.Equals(other)
            && EqualityComparer<T>.Default.Equals(Value, other.Value);
    }

    public bool Equals(T? other)
    {
        return EqualityComparer<T>.Default.Equals(Value, other);
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals(( Literal<T> ) obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(base.GetHashCode(), Value);
    }
}