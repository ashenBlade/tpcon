namespace JsUtils.Implementation.JsTokens;

public class JsIdentifier : JsToken
{
    public JsIdentifier(string name) : base(name, JsTokenType.Identifier) { }

    public override bool Equals(JsToken? other)
    {
        return other is JsIdentifier and not null && Name == other.Name;
    }
}