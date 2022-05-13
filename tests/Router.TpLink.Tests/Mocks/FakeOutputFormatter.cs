using Router.Commands;

namespace Router.TpLink.Tests;

public class FakeOutputFormatter : IOutputFormatter
{
    public string Format<TFormattable>(TFormattable formattable) where TFormattable : class
    {
        throw new MustNotBeCalledException();
    }
}