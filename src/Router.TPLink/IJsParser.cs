using JsTypes;

namespace Router.TPLink.JsParser;

public interface IJsParser
{
    public JsScript ParseScript(string script);
}