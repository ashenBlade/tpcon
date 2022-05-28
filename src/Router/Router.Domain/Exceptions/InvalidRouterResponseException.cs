namespace Router.Domain.Exceptions;

public class InvalidRouterResponseException : RouterException
{
    public InvalidRouterResponseException(string message = "") 
        : base(message)
    { }
}