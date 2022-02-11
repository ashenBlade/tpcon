using JsTypes;
using JsUtils.Implementation.JsTokens;

namespace JsUtils.Implementation;

public static class TokenExtensions
{
    public static JsTypes.JsType ToJsType(this JsTokens.JsType type)
    {
        return type switch
               {
                   JsTokens.JsNumber number               => new JsTypes.JsNumber(number.Value),
                   JsTokens.JsStringLiteral stringLiteral => new JsString(stringLiteral.Value),
                   JsTokens.JsRegex regex                 => new JsTypes.JsRegex(regex.Value),
                   _                                      => throw new Exception($"Unknown base type {type.Name}")
               };
    } 
}