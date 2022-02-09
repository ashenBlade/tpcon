namespace JsUtils.Implementation.JsTokens;

public class JsVar : JsToken
{
    public JsVar() : base(string.Empty, JsTokenType.Var) { }
    public override bool Equals(JsToken? other)
    {
        return other is JsVar and not null;
    }
}