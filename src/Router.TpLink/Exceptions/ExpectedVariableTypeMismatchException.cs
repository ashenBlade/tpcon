using Router.Domain.Exceptions;

namespace Router.TpLink.Exceptions;

public class ExpectedVariableTypeMismatchException : InvalidRouterResponseException
{
    public ExpectedVariableTypeMismatchException(string variableName, Type expected, Type actual, string? message = null)
        : base(message ?? $"Variable \"{variableName}\" mismatch expected type\nExpected: {expected}\nActual: {actual}")
    {
        VariableName = variableName;
        Expected = expected;
        Actual = actual;
    }

    public string VariableName { get; }
    public Type Expected { get; }
    public Type Actual { get; }
    
}