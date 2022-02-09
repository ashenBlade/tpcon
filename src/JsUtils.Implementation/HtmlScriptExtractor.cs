using System.Text;
using System.Text.RegularExpressions;

namespace JsUtils.Implementation;

public class HtmlScriptExtractor : IScriptExtractor
{
    public IEnumerable<string> ExtractScripts(string source)
    {
        var regex = new Regex("<([Ss][Cc][Rr][Ii][Pp][Tt])(.|\n)*?>(?<Content>(.|\n)*?)</([Ss][Cc][Rr][Ii][Pp][Tt])>");
        var matches = regex.Matches(source);
        return matches.Select(match => match.Groups["Content"].Value);
    }
}