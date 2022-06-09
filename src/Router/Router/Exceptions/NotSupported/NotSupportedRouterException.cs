namespace Router.Exceptions.NotSupported;

public abstract class NotSupportedRouterException : RouterException
{
    protected NotSupportedRouterException(string? message) : base(message)
    { }
}