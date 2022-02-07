namespace JsTypes;

public class JsScript
{
    private readonly Dictionary<string, JsVariable> _variables = new();
    public JsScript(IEnumerable<JsVariable> variables) 
        : this(variables.ToList())
    { }

    public IEnumerable<JsVariable> GetAllVariables() => _variables.Values;
    public JsVariable this[string name] => _variables[name];

    public JsScript(List<JsVariable> variables)
    {
        foreach (var variable in variables)
        {
            _variables.Add(variable.Name, (JsVariable) variable.Clone());
        }
    }
}