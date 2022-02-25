namespace JsTypes;

public class JsArray : JsObject, IEnumerable<JsType>, IEquatable<JsArray>
{
    public int Count => _values.Count;
    public IEnumerable<JsType> Values => _values;
    
    private readonly List<JsType> _values;
    public JsArray()
    {
        _values = new List<JsType>();
    }
    
    public void Add(JsType type)
    {
        _values.Add(type);
    }

    
    public JsType this[int index]
    {
        get => _values[index];
        set => _values[index] = value;
    }

    public new IEnumerator<JsType> GetEnumerator()
    {
        return _values.GetEnumerator();
    }


    public bool Equals(JsArray? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return _values.Equals(other._values);
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals(( JsArray ) obj);
    }

    public override int GetHashCode()
    {
        return _values.GetHashCode();
    }
}