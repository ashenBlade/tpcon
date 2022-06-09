namespace JsTypes;

public class JsVariable : JsType, ICloneable, IEquatable<JsVariable>
{
    public string Name { get; }
    public JsType Value { get; }

    public JsVariable(string name, JsType value)
    {
        Name = name;
        Value = value ?? throw new ArgumentNullException(nameof(value), "To use null as value of js variable use JsNull type");
    }

    public override object Clone()
    {
        return new JsVariable(Name, ( JsType ) Value.Clone());
    }

    public override bool Equals(JsType? other)
    {
        return other is JsVariable variable && Equals(variable);
    }

    public bool Equals(JsVariable? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return Value.Equals(other.Value)
            && Name == other.Name;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals(( JsVariable ) obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Value, Name);
    }

    public override string? ToString()
    {
        return Value.ToString();
    }
}