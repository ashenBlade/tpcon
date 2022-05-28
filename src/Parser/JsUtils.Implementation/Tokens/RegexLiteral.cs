namespace JsUtils.Implementation.Tokens;

public class RegexLiteral: Literal<string>
{
    public static Token Token => new(Tags.Regex);
    public RegexLiteral(string regex) : base(regex, Tags.Regex)
    { }
}