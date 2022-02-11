using Microsoft.VisualBasic.CompilerServices;

namespace JsTypes;

public class JsString : JsType
{
    public JsString(string value)
    {
        Value = value ?? throw new ArgumentNullException();
    }
    public string Value { get; }
    public override object Clone()
    {
        return new JsString(Value);
    }

    public override bool Equals(JsType? other)
    {
        return other is JsString jsString && jsString.Value == Value;
    }
}