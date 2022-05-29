using JsTypes;

namespace JsUtils.Implementation;

public static class JsTypeExtensions
{
    public static int GetInt(this JsType type)
    {
        return (int) ( ( JsNumber ) type ).Value;
    }

    public static string GetString(this JsType type)
    {
        return ( ( JsString ) type ).Value;
    }
}