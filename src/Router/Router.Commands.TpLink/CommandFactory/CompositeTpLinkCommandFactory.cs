using Router.Commands.CommandLine.Exceptions;

namespace Router.Commands.TpLink.CommandFactory;

public abstract class CompositeTpLinkCommandFactory : BaseTpLinkCommandFactory
{
    private readonly IEnumerable<KeyValuePair<string, Func<BaseTpLinkCommandFactory>>> _commands;

    protected CompositeTpLinkCommandFactory(IEnumerable<KeyValuePair<string, Func<BaseTpLinkCommandFactory>>> commands)
    {
        ArgumentNullException.ThrowIfNull(commands);
        _commands = commands;
    }

    public override IRouterCommand CreateRouterCommand(RouterCommandContext context)
    {
        var currentCommand = context.CurrentCommand;
        if (currentCommand is null)
        {
            throw new IncompleteCommandException(context.Command.ToArray());
        }

        var func = _commands
                  .FirstOrDefault(c => c.Key == currentCommand)
                  .Value;
        if (func is null)
        {
            throw new UnknownCommandException(currentCommand, context.Command.ToArray());
        }

        context.MoveNext();
        return func().CreateRouterCommand(context);
    }
}