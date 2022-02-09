namespace JsUtils.Implementation.JsTokens;

public class JsStringLiteral : JsToken
{
    public string Value { get; }
    
    public JsStringLiteral(string value) : base("string literal", JsTokenType.String)
    {
        Value = value;
    }
}