namespace JsParser.Core.Types;

public class JsNull : JsType
{
    public static readonly JsNull Instance = new();
    private JsNull() { }
}