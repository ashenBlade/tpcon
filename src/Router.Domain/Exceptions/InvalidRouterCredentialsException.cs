namespace Router.Domain.Exceptions;

public class InvalidRouterCredentialsException : RouterException
{
    public string Username { get; }
    public string Password { get; }

    public InvalidRouterCredentialsException(string username, string password, string? message = null)
    : base(message)
    {
        Username = username;
        Password = password;
    }
}