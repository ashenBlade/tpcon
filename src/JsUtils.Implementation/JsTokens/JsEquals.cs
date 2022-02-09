namespace JsUtils.Implementation.JsTokens;

public class JsEquals : JsToken
{
    public JsEquals() : base(string.Empty, JsTokenType.Equals) { }
    public override bool Equals(JsToken? other)
    {
        return other is JsEquals and not null;
    }
}