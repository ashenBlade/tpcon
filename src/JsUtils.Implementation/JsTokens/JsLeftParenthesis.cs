namespace JsUtils.Implementation.JsTokens;

public class JsLeftParenthesis : JsToken
{
    public JsLeftParenthesis() : base("(", JsTokenType.LeftParenthesis) { }
    public override bool Equals(JsToken? other)
    {
        return other is JsLeftParenthesis and not null;
    }
}