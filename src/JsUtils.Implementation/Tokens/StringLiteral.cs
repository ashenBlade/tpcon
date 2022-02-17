namespace JsUtils.Implementation.Tokens;

public class StringLiteral: Literal<string>
{
    public StringLiteral(string value) : base(value, Tags.String)
    { }
}