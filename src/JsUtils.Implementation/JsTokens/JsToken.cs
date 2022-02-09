using System.Text.Json;

namespace JsUtils.Implementation.JsTokens;

public abstract class JsToken
{
    public string Name { get; }
    public JsTokenType TokenType { get; }
    protected JsToken(string name, JsTokenType tokenType)
    {
        Name = name;
        TokenType = tokenType;
    }
}