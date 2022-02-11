namespace JsTypes;

public class JsNumber : JsType
{
    public JsNumber(decimal value)
    {
        Value = value;
    }
    
    public decimal Value { get; }
    public override object Clone()
    {
        return new JsNumber(Value);
    }

    public override bool Equals(JsType? other)
    {
        return other is JsNumber number && number.Value == Value;
    }
}