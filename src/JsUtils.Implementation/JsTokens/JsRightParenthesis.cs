namespace JsUtils.Implementation.JsTokens;

public class JsRightParenthesis : JsToken
{
    public JsRightParenthesis() : base(")", JsTokenType.RightParenthesis) { }
    public override bool Equals(JsToken? other)
    {
        return other is JsRightParenthesis and not null;
    }
}