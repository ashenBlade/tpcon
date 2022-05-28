using JsTypes;

namespace JsUtils;

public interface IJsVariableExtractor
{
    public IEnumerable<JsVariable> ExtractVariables(string script);
}