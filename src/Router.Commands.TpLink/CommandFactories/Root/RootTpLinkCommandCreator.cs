using Router.Commands.Exceptions;
using Router.Commands.TpLink.CommandFactories.Wlan;

namespace Router.Commands.TpLink.CommandFactories;

internal class RootTpLinkCommandCreator : CompositeTpLinkCommandCreator
{
    public RootTpLinkCommandCreator(IEnumerable<InternalTpLinkCommandCreator> commands)
        : base(commands, String.Empty)
    { }
    
    public RootTpLinkCommandCreator() 
        : this(new InternalTpLinkCommandCreator[]
               {
                   new WlanCompositeTpLinkCommandCreator(),
                   new RefreshTpLinkCommandCreator(),
                   new CheckConnectionTpLinkCommandCreator()
               }) 
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