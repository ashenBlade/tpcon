namespace JsTypes;

public class JsVariable : ICloneable, IEquatable<JsVariable>
{
    private JsType _value;
    public string Name { get; }
    public JsType Value
    {
        get => _value;
        set => _value = value ?? JsNull.Instance;
    }

    public JsVariable(string name, JsType? value = null)
    {
        Name = name;
        _value = value ?? JsNull.Instance;
    }

    public object Clone()
    {
        return new JsVariable(Name, ( JsType ) _value.Clone());
    }

    public bool Equals(JsVariable? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return _value.Equals(other._value)
            && Name == other.Name;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals(( JsVariable ) obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(_value, Name);
    }
}