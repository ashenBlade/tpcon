namespace JsUtils.Implementation.JsTokens;

public class JsIf : JsToken
{
    public JsIf() : base("if", JsTokenType.If) 
    { }

    public override bool Equals(JsToken? other)
    {
        return other is JsIf and not null;
    }
}