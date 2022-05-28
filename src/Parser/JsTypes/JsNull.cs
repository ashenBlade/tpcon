namespace JsTypes;

public class JsNull : JsType
{
    public static readonly JsNull Instance = new();
    private JsNull() { }
    public override object Clone()
    {
        return Instance;
    }

    public override bool Equals(JsType? other)
    {
        return other is JsNull;
    }
}