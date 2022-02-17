namespace JsUtils.Implementation.Tokens;

public class Number : Literal<decimal>
{
    public Number(decimal value) : base(value, Tags.Number)
    { }
}