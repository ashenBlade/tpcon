namespace JsUtils;

public interface IScriptExtractor
{
    public IEnumerable<string> ExtractScripts(string source);
}