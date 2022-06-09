using Router.Commands;

namespace TpLinkConsole.Infrastructure;

public class TLWR741NDTpLinkRouterApplication : IRouterApplication
{
    private readonly ICommandLineContextParser _parser;
    private readonly IRouterCommandFactory _factory;

    public TLWR741NDTpLinkRouterApplication(ICommandLineContextParser parser, IRouterCommandFactory factory)
    {
        _parser = parser;
        _factory = factory;
    }

    public async Task RunAsync(string[] args)
    {
        var cmd = _parser.ParseCommandLineContext(args);
        var formatter = cmd.GetOutputFormatter();
        var context = new RouterCommandContext(cmd.Command, cmd.Arguments, formatter, Console.Out, cmd.RouterConnectionParameters);
        var command = _factory.CreateRouterCommand(context);
        await command.ExecuteAsync();
    }
}