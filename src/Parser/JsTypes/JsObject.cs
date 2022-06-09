using System.Collections;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace JsTypes;

public class JsObject : JsType, IEnumerable<KeyValuePair<string, JsType>>, IEquatable<JsObject>
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

        return other is JsObject jsObject
            && _types.Count == jsObject._types.Count
            && !_types.Except(jsObject._types).Any();
    }
    
    public IEnumerator<KeyValuePair<string, JsType>> GetEnumerator()
    {
        return _types.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public IEnumerable<KeyValuePair<string, JsType>> Values => _types;

    public bool Equals(JsObject? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return _types.Equals(other._types);
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals(( JsObject ) obj);
    }

    public override int GetHashCode()
    {
        return _types.GetHashCode();
    }

    public override string ToString()
    {
        return JsonSerializer.Serialize(_types);
    }
}