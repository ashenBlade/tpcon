namespace JsUtils.Implementation.Tokens;

public class RegexLiteral: Literal<string>
{
    public RegexLiteral(string regex) : base(regex, Tags.Regex)
    { }
}