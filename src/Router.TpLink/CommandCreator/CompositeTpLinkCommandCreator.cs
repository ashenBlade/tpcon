using Router.Commands;
using Router.Commands.Exceptions;


namespace Router.TpLink.CommandCreator;

public abstract class CompositeTpLinkCommandCreator : TpLinkCommandCreator
{
    protected Dictionary<string, TpLinkCommandCreator> Commands { get; }

    protected CompositeTpLinkCommandCreator(IEnumerable<TpLinkCommandCreator> commands, string rootName)
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