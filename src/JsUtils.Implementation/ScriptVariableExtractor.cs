using JsTypes;

namespace JsUtils.Implementation;

public class ScriptVariableExtractor : IJsVariableExtractor
{
    private readonly ITokenizer _tokenizer;

    public ScriptVariableExtractor(ITokenizer tokenizer)
    {
        _tokenizer = tokenizer;
    }
    
    public IEnumerable<JsVariable> ExtractVariables(string script)
    {
        foreach (var token in _tokenizer.Tokenize(script))
        {
            yield break;
        }
    }
}