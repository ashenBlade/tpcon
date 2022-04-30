namespace Router.Domain.Exceptions;

public class RouterException : Exception
{
    public RouterException(string? message = null)
        : base(message)
    { }
}