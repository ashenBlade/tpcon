namespace Router.Exceptions.NotSupported;

public class FunctionalityNotSupportedRouterException : NotSupportedRouterException
{
    public FunctionalityNotSupportedRouterException(string? message) : base(message)
    { }
}