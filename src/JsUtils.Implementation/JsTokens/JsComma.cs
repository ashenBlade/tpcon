namespace JsUtils.Implementation.JsTokens;

public class JsComma : JsToken
{
    public JsComma() : base(",", JsTokenType.Comma) { }
    public override bool Equals(JsToken? other)
    {
        return other is JsComma and not null;
    }
}