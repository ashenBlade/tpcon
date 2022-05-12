using Router.Commands.Exceptions;
using Router.Commands.TpLink.CommandFactories.Wlan;

namespace Router.Commands.TpLink.CommandFactories;

internal class RootTpLinkCommandCreator : CompositeTpLinkCommandCreator
{
    public RootTpLinkCommandCreator(IEnumerable<InternalTpLinkCommandCreator> commands)
        : base(commands, string.Empty)
    { }
    
    public override IRouterCommand CreateRouterCommand(RouterCommandContext lineContext)
    {
        var currentCommand = lineContext.CurrentCommand;
        if (!Commands.TryGetValue(currentCommand, out var factory)) 
            throw new UnknownCommandLineException();
        lineContext.MoveNext();
        return factory.CreateRouterCommand(lineContext);
    }
}