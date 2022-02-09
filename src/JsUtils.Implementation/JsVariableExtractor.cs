using JsTypes;

namespace JsUtils.Implementation;

public class JsVariableExtractor : IJsVariableExtractor
{
    private ITokenizer Tokenizer { get; }

    public JsVariableExtractor(ITokenizer tokenizer)
    {
        Tokenizer = tokenizer;
    }
    
    public IEnumerable<JsVariable> ExtractVariables(string script)
    {
        return FindVariablesInScript(script);
    }

    private IEnumerable<JsVariable> FindVariablesInScript(string script)
    {
        return Array.Empty<JsVariable>();
    }
}