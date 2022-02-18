namespace JsUtils.Implementation.Tokens;

public class BoolLiteral: Literal<bool>
{
    public static readonly BoolLiteral True = new(true);

    public static readonly BoolLiteral False = new(false);
    public BoolLiteral(bool value) : base(value, value ? Tags.True : Tags.False)
    { }
}