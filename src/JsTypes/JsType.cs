namespace JsTypes;

public abstract class JsType : ICloneable, IEquatable<JsType>
{
    public abstract object Clone();
    public abstract bool Equals(JsType? other);
    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals(( JsType ) obj);
    }
}