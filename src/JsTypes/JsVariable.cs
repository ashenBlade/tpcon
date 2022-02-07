namespace JsTypes;

public class JsVariable : ICloneable
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
}