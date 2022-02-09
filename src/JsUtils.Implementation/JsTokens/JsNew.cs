namespace JsUtils.Implementation.JsTokens;

public class JsNew : JsToken
{
    public JsNew() : base("new", JsTokenType.New) { }
    public override bool Equals(JsToken? other)
    {
        return other is JsNew and not null;
    }
}