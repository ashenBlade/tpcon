using System.Collections;

namespace JsTypes;

public class JsObject : JsType, IEnumerable<KeyValuePair<string, JsType>>
{
    private readonly Dictionary<string, JsType> _types = new();

    public JsObject() { }

    private JsObject(Dictionary<string, JsType> dictionary)
    {
        foreach (var (key, type) in dictionary)
        {
            _types.Add(key, (JsType) type.Clone());
        }    
    }

    public JsType this[string key]
    {
        get => _types.TryGetValue(key, out var type)
                   ? type
                   : JsUndefined.Instance;
        set => _types[key] = value;
    }

    public override object Clone()
    {
        return new JsObject(_types);
    }

    public override bool Equals(JsType? other)
    {
        return other is JsObject jsObject && jsObject._types.Equals(_types);
    }
    
    public IEnumerator<KeyValuePair<string, JsType>> GetEnumerator()
    {
        return _types.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}