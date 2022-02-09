namespace JsUtils.Implementation.JsTokens;

public class JsFor : JsToken
{
    public JsFor() : base("for", JsTokenType.For) 
    { }

    public override bool Equals(JsToken? other)
    {
        return other is JsFor and not null;
    }
}