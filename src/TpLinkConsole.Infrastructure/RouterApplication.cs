using Router.Commands;

namespace TpLinkConsole.Infrastructure;

public class RouterApplication : IApplication
{
    private readonly ICommandLineContextParser _parser;
    private readonly IRouterCommandFactory _factory;

    public RouterApplication(ICommandLineContextParser parser, IRouterCommandFactory factory)
    {
        _parser = parser;
        _factory = factory;
    }

    public async Task RunAsync(string[] args)
    {
        var context = _parser.ParseCommandLineContext(args);
        var command = _factory.CreateRouterCommand(context);
        await command.ExecuteAsync();
    }
}