namespace TpLinkConsole.Infrastructure;

public interface IRouterApplication
{
    Task RunAsync(string[] args);
}