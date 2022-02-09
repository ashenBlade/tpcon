namespace JsUtils.Implementation.JsTokens;

public class JsWhile : JsToken
{
    public JsWhile() : base("while", JsTokenType.While) 
    { }

    public override bool Equals(JsToken? other)
    {
        return other is JsWhile and not null;
    }
}