namespace JsTypes;

public class JsUndefined : JsType
{
    public static readonly JsUndefined Instance = new();
    private JsUndefined() { }
    public override object Clone()
    {
        return Instance;
    }

    public override bool Equals(JsType? other)
    {
        return ReferenceEquals(this, other);
    }

    public override string ToString()
    {
        return "undefined";
    }
}