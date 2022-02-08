namespace Router.Domain;

public abstract class RemoteRouter : Router
{
    public abstract Task<bool> CheckConnectionAsync();
    public string Username { get; }
    public string Password { get; }
    public Uri Address { get; }

    protected RemoteRouter(string username, string password, Uri address)
    {
        Username = username ?? throw new ArgumentNullException(nameof(username));
        Password = password ?? throw new ArgumentNullException(nameof(password));
        Address = address ?? throw new ArgumentNullException(nameof(address));
    }
}