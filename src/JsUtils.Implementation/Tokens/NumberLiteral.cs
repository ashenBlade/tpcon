namespace JsUtils.Implementation.Tokens;

public class NumberLiteral : Literal<decimal>
{
    public NumberLiteral(decimal value) : base(value, Tags.Number)
    { }
}