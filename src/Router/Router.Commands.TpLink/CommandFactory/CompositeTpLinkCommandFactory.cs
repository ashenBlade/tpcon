using Router.Commands.CommandLine.Exceptions;

namespace Router.Commands.TpLink.CommandFactory;

public abstract class CompositeTpLinkCommandFactory : TpLinkCommandFactory
{
    private Dictionary<string, TpLinkCommandFactory> Commands { get; }

    protected CompositeTpLinkCommandFactory(IEnumerable<TpLinkCommandFactory> commands, string rootName)
        : base(rootName)
    {
        ArgumentNullException.ThrowIfNull(commands);
        Commands = commands.ToDictionary(c => c.Name);
    }

    public override IRouterCommand CreateRouterCommand(RouterCommandContext context)
    {
        var currentCommand = context.CurrentCommand;
        if (currentCommand is null)
        {
            throw new IncompleteCommandException(context.Command.ToArray());
        }
        if (!Commands.TryGetValue(currentCommand, out var factory))
            throw new UnknownCommandException(currentCommand, context.Command.ToArray());
        context.MoveNext();
        return factory.CreateRouterCommand(context);

    }
}