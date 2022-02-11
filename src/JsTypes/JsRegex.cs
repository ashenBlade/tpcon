namespace JsTypes;

public class JsRegex : JsType
{
    public string Regex { get; }

    public JsRegex(string regex)
    {
        Regex = regex;
    }
    public override object Clone()
    {
        return new JsRegex(Regex);
    }

    public override bool Equals(JsType? other)
    {
        return other is JsRegex regex && regex.Regex == Regex;
    }
}