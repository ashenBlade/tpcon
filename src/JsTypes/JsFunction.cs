namespace JsTypes;

public class JsFunction : JsType
{
    public string Name { get; }
    public string Body { get; }

    public JsFunction(string name, string body)
    {
        Name = name;
        Body = body;
    }

    public override object Clone()
    {
        return new JsFunction(Name, Body);
    }
}