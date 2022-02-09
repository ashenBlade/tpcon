namespace JsUtils.Implementation.JsTokens;

public class JsIdentifier : JsToken
{
    public JsIdentifier(string name) : base(name, JsTokenType.Identifier) { }
}