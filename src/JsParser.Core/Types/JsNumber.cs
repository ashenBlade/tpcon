namespace JsParser.Core.Types;

public class JsNumber : JsType
{
    public JsNumber(decimal value)
    {
        Value = value;
    }
    
    public decimal Value { get; }
}