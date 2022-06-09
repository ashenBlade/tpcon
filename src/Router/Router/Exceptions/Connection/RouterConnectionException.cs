namespace Router.Exceptions;

public abstract class RouterConnectionException : RouterException
{
    protected RouterConnectionException(string? message) 
        : base(message)
    { }
}