using System.Reflection.Metadata.Ecma335;

namespace JsUtils.Implementation.JsTokens;

public class JsDo : JsToken
{
    public JsDo() : base("do", JsTokenType.Do) 
    { }

    public override bool Equals(JsToken? other)
    {
        return other is JsDo and not null;
    }
}