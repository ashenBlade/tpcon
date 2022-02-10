namespace JsUtils.Implementation.JsTokens;

public class JsRightBrace : JsToken
{
    public JsRightBrace() : base("}", JsTokenType.RightCurveBracket) { }
    public override bool Equals(JsToken? other)
    {
        return other is JsRightBrace and not null;
    }
}