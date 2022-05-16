namespace JsUtils.Implementation.Tokens;

public class NumberLiteral : Literal<decimal>
{
    public static Token Token => new(Tags.Number);
    public NumberLiteral(decimal value) : base(value, Tags.Number)
    { }
}