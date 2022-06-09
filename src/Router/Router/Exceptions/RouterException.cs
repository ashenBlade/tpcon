namespace Router.Exceptions;

public abstract class RouterException : Exception
{
    public RouterException(string? message)
        : base(message)
    { }
}