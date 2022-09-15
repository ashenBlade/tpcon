using Router.Exceptions;

namespace Router.Commands.TpLink.Exceptions;

public class ExpectedVariableTypeMismatchException : InvalidRouterResponseException
{
    public ExpectedVariableTypeMismatchException(string variableName,
                                                 Type expected,
                                                 Type actual,
                                                 string? message = null)
        : base(message
            ?? $"Ожидаемый тип переменной \"{variableName}\" - {expected}\n"
             + $"Получено: {actual}")
    {
        VariableName = variableName;
        Expected = expected;
        Actual = actual;
    }

    public string VariableName { get; }
    public Type Expected { get; }
    public Type Actual { get; }
}