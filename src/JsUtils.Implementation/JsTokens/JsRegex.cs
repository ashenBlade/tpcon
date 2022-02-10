namespace JsUtils.Implementation.JsTokens;

public class JsRegex : JsToken
{
    public string Value { get; }

    public JsRegex(string value) : base("regex", JsTokenType.Regex)
    {
        Value = value;
    }
}