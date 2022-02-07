using Microsoft.VisualBasic.CompilerServices;

namespace JsTypes;

public class JsString : JsType
{
    public JsString(string value)
    {
        Value = value ?? throw new ArgumentNullException();
    }
    public string Value { get; }
}