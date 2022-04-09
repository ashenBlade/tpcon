namespace Router.Commands.Implementation.Commands;

public class PrintErrorCommand : IRouterCommand
{
    private readonly string _message;

    public PrintErrorCommand(string message)
    {
        _message = message;
    }
    public Task ExecuteAsync()
    {
        Console.WriteLine(_message);
        return Task.CompletedTask;
    }
}