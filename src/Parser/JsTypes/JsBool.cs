namespace JsTypes;

public class JsBool : JsType
{
    public static JsBool True => new JsBool(true);
    public static JsBool False => new JsBool(false);
    public bool Value { get; }

    public JsBool(bool value)
    {
        Value = value;
    }
    
    public override object Clone()
    {
        return new JsBool(Value);
    }

    public override bool Equals(JsType? other)
    {
        return other is JsBool jsBool && jsBool.Value == Value;
    }
}