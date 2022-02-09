namespace JsUtils.Implementation.JsTokens;

public class JsSemicolon : JsToken
{
    public JsSemicolon() : base(";", JsTokenType.Semicolon) { }
    public override bool Equals(JsToken? other)
    {
        return other is JsSemicolon and not null;
    }
}