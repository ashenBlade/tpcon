using Router.Commands;
using Router.Commands.Exceptions;

namespace Router.TpLink.CommandFactories.Root;

internal class RootTpLinkCommandFactory : CompositeTpLinkCommandFactory
{
    public RootTpLinkCommandFactory(IEnumerable<InternalTpLinkCommandFactory> commands)
        : base(commands, string.Empty)
    { }
    
    public override IRouterCommand CreateRouterCommand(RouterCommandContext lineContext)
    {
        var currentCommand = lineContext.CurrentCommand;
        if (!Commands.TryGetValue(currentCommand, out var factory)) 
            throw new UnknownCommandException();
        lineContext.MoveNext();
        return factory.CreateRouterCommand(lineContext);
    }
}