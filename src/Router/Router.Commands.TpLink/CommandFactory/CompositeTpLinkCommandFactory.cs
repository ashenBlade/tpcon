using Router.Commands.CommandLine.Exceptions;
using Router.Commands.TpLink.Commands;

namespace Router.Commands.TpLink.CommandFactory;

public abstract class CompositeTpLinkCommandFactory : BaseTpLinkCommandFactory
{
    private readonly IEnumerable<KeyValuePair<CommandFactoryInfo, Func<BaseTpLinkCommandFactory>>> _commands;

    protected CompositeTpLinkCommandFactory(
        IEnumerable<KeyValuePair<CommandFactoryInfo, Func<BaseTpLinkCommandFactory>>> commands)
    {
        ArgumentNullException.ThrowIfNull(commands);
        _commands = commands;
    }

    public override IRouterCommand CreateRouterCommand(RouterCommandContext context)
    {
        IRouterCommand GetHelpCommand()
        {
            var commandText = string.Join(' ', context.CommandUntil);
            return new PrintHelpCommand(context.OutputWriter,
                                        commandText,
                                        CommandsDescription);
        }

        if (context.IsLastCommand && context.Arguments.ContainsKey("help"))
        {
            return GetHelpCommand();
        }

        var currentCommand = context.CurrentCommand;
        if (currentCommand is null)
        {
            throw new IncompleteCommandException(context.Command.ToArray());
        }

        var pair = _commands
           .FirstOrDefault(c => c.Key.Name == currentCommand);
        var func = pair.Value;

        if (func is null)
        {
            throw new UnknownCommandException(currentCommand, context.Command.ToArray());
        }

        // if (context.Arguments.ContainsKey("help") && context.IsLastCommand)
        // {
        //     return GetHelpCommand(func());
        // }


        context.MoveNext();
        return func().CreateRouterCommand(context);
    }

    protected IEnumerable<string> GetCommandDescriptions()
    {
        return _commands
              .Select(c => c.Key)
              .Select(p => $"{p.Name} - {p.Description}");
    }

    private string CommandsDescription => string.Join('\n', GetCommandDescriptions());

    protected static KeyValuePair<CommandFactoryInfo, Func<BaseTpLinkCommandFactory>> Command(
        string name,
        string description,
        Func<BaseTpLinkCommandFactory> func) => new(new(name, description), func);
}