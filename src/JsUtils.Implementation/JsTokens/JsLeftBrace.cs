namespace JsUtils.Implementation.JsTokens;

public class JsLeftBrace : JsToken
{
    public JsLeftBrace() : base("{", JsTokenType.LeftCurveBracket) { }
    public override bool Equals(JsToken? other)
    {
        return other is JsLeftBrace and not null;
    }
}