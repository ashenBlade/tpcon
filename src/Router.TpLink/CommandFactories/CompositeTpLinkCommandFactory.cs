using Router.Commands;
using Router.Commands.Exceptions;

namespace Router.TpLink.CommandFactories;

internal abstract class CompositeTpLinkCommandFactory : InternalTpLinkCommandFactory
{
    protected Dictionary<string, InternalTpLinkCommandFactory> Commands { get; }

    protected CompositeTpLinkCommandFactory(IEnumerable<InternalTpLinkCommandFactory> commands, string rootName)
        : base(rootName)
    {
        ArgumentNullException.ThrowIfNull(commands);
        Commands = commands.ToDictionary(c => c.Name);
    }

    public override IRouterCommand CreateRouterCommand(RouterCommandContext context)
    {
        var currentCommand = context.CurrentCommand;
        if (!Commands.TryGetValue(currentCommand, out var factory))
            throw new UnknownCommandException();
        context.MoveNext();
        return factory.CreateRouterCommand(context);

    }
}