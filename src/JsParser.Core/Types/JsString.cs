using Microsoft.VisualBasic.CompilerServices;

namespace JsParser.Core.Types;

public class JsString : JsType
{
    public JsString(string value)
    {
        Value = value ?? throw new ArgumentNullException();
    }
    public string Value { get; }
}