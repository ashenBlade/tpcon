using Router.Commands.Exceptions;

namespace Router.Commands.TpLink.CommandFactories;

internal class CompositeTpLinkCommand : InternalTpLinkCommand
{
    protected Dictionary<string, InternalTpLinkCommand> Commands { get; }

    protected CompositeTpLinkCommand(IEnumerable<InternalTpLinkCommand> commands, string rootName)
        : base(rootName)
    {
        ArgumentNullException.ThrowIfNull(commands);
        Commands = commands.ToDictionary(c => c.Name);
    }

    public override IRouterCommand CreateRouterCommand(CommandContext context)
    {
        var currentCommand = context.CurrentCommand;
        if (!Commands.TryGetValue(currentCommand, out var factory))
            throw new UnknownCommandException(context.Command);
        context.MoveNext();
        return factory.CreateRouterCommand(context);

    }

    public override void WriteHelpTo(TextWriter writer)
    {
        throw new NotImplementedException();
    }
}