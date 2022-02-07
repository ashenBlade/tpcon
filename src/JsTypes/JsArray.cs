namespace JsTypes;

public class JsArray : JsObject
{
    private readonly List<JsType> _values;
    public JsArray()
    {
        _values = new List<JsType>();
    }
    
    public void Add(JsType type)
    {
        _values.Add(type);
    }

    public int Count => _values.Count;
    
    public JsType this[int index]
    {
        get => _values[index];
        set => _values[index] = value;
    }
}