namespace JsUtils.Implementation.JsTokens;

public class JsNumber : JsType
{
    public decimal Value { get; }

    public JsNumber(decimal value) : base("number", JsTokenType.Number)
    {
        Value = value;
    }

    public override bool Equals(JsToken? other)
    {
        return other is JsNumber number and not null && number.Value == Value;
    }
}