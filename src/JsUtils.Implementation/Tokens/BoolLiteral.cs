namespace JsUtils.Implementation.Tokens;

public class BoolLiteral: Literal<bool>
{
    public BoolLiteral(bool value) : base(value, Tags.Bool)
    { }
}