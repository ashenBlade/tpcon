namespace JsTypes;

public class JsUndefined : JsType
{
    public static readonly JsUndefined Instance = new();
    private JsUndefined() { }
    public override object Clone()
    {
        return Instance;
    }
}