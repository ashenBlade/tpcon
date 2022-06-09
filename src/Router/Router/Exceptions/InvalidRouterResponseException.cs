namespace Router.Exceptions;

public abstract class InvalidRouterResponseException : RouterException
{
    public InvalidRouterResponseException(string? message = null) 
        : base(message)
    { }
}