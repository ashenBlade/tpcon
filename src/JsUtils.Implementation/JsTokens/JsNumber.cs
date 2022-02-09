namespace JsUtils.Implementation.JsTokens;

public class JsNumber : JsToken
{
    public decimal Value { get; }

    public JsNumber(decimal value) : base("decimal", JsTokenType.Number)
    {
        Value = value;
    }
}