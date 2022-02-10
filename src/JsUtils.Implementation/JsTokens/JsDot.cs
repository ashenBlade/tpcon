namespace JsUtils.Implementation.JsTokens;

public class JsDot : JsToken
{
    public JsDot() : base(".", JsTokenType.Dot) { }
    public override bool Equals(JsToken? other)
    {
        return other is JsDot and not null;
    }
}