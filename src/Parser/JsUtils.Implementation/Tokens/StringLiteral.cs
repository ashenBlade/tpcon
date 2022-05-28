namespace JsUtils.Implementation.Tokens;

public class StringLiteral: Literal<string>
{
    public static Token Token => new(Tags.String);
    public StringLiteral(string value) : base(value, Tags.String)
    { }
}