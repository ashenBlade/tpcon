namespace JsTypes;

public class JsObject : JsType
{
    private readonly Dictionary<string, JsType> _types = new();

    public JsType this[string key]
    {
        get => _types.TryGetValue(key, out var type)
                   ? type
                   : JsUndefined.Instance;
        set => _types[key] = value;
    }
}