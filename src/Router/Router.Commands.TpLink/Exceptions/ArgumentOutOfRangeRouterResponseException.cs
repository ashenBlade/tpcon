using Router.Exceptions;

namespace Router.Commands.TpLink.Exceptions;

public class ArgumentOutOfRangeRouterResponseException : InvalidRouterResponseException
{
    public string Argument { get; }
    public object Actual { get; }
    public string Expected { get; }

    public ArgumentOutOfRangeRouterResponseException(string argument,
                                                     object actual,
                                                     string expected,
                                                     string? message = null) :
        base(message ?? GetDefaultErrorMessage(argument, actual, expected))
    {
        Argument = argument;
        Actual = actual;
        Expected = expected;
    }

    private static string GetDefaultErrorMessage(string argument, object actual, string expected)
        =>
            $"Router responded with invalid values.\nArgument \"{argument}\" expected to be: {expected}\nBut given: {actual}";
}