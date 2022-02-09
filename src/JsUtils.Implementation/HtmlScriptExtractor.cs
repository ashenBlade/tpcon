using System.Text;
using System.Text.RegularExpressions;

namespace JsUtils.Implementation;

public class HtmlScriptExtractor : IScriptExtractor
{
    public string ExtractScript(string source)
    {
        var regex = new Regex("<script(.|\n)*?>(?<Content>(.|\n)*?)</script>");
        var matches = regex.Matches(source);
        var scripts = new List<string>();
        foreach (Match match in matches)
        {
            var value = match.Groups["Content"].Value;
            if (value != string.Empty)
            {
                scripts.Add(value);
            }
        }
        
        return scripts.Count == 0 
                   ? string.Empty 
                   : scripts.Aggregate((s, n) => $"{s}\n{n}");
    }
}