using JsTypes;

namespace JsUtils.Implementation;

public class JsVariableExtractor : IJsVariableExtractor
{
    public IEnumerable<JsVariable> ExtractVariables(string script)
    {
        return Array.Empty<JsVariable>();
    }
}