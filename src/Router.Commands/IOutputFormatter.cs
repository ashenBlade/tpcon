namespace Router.Commands;

public interface IOutputFormatter
{
    public string Format<TFormattable>(TFormattable formattable)
        where TFormattable : class;
}