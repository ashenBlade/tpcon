using Router.Commands;
using Router.Commands.Exceptions;

namespace Router.TpLink.CommandFactories;

internal abstract class CompositeTpLinkCommandCreator : InternalTpLinkCommandCreator
{
    protected Dictionary<string, InternalTpLinkCommandCreator> Commands { get; }

    protected CompositeTpLinkCommandCreator(IEnumerable<InternalTpLinkCommandCreator> commands, string rootName)
        : base(rootName)
    {
        ArgumentNullException.ThrowIfNull(commands);
        Commands = commands.ToDictionary(c => c.Name);
    }

    public override IRouterCommand CreateRouterCommand(RouterCommandContext context)
    {
        var currentCommand = context.CurrentCommand;
        if (!Commands.TryGetValue(currentCommand, out var factory))
            throw new UnknownCommandLineException();
        context.MoveNext();
        return factory.CreateRouterCommand(context);

    }
}