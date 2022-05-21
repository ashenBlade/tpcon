namespace TpLinkConsole.Infrastructure;

public interface IApplication
{
    Task RunAsync(string[] args);
}