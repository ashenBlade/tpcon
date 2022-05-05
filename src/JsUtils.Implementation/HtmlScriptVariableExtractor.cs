using JsTypes;

namespace JsUtils.Implementation;

public class HtmlScriptVariableExtractor : IJsVariableExtractor
{
    private readonly IScriptExtractor _scriptExtractor;
    private readonly IJsVariableExtractor _variableExtractor;

    public HtmlScriptVariableExtractor(IScriptExtractor scriptExtractor, IJsVariableExtractor variableExtractor)
    {
        _scriptExtractor = scriptExtractor;
        _variableExtractor = variableExtractor;
    }
    public IEnumerable<JsVariable> ExtractVariables(string source)
    {
        return _scriptExtractor.ExtractScripts(source)
                               .SelectMany(s => _variableExtractor
                                              .ExtractVariables(s));
    }
}