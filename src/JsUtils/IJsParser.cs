using JsTypes;

namespace JsUtils;

public interface IJsParser
{
    public JsScript ParseScript(string script);
}