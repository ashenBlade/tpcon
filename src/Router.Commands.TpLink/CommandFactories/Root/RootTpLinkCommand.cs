using Router.Commands.Exceptions;
using Router.Commands.TpLink.CommandFactories.Wlan;

namespace Router.Commands.TpLink.CommandFactories;

internal class RootTpLinkCommand : CompositeTpLinkCommand
{
    public RootTpLinkCommand(IEnumerable<InternalTpLinkCommand> commands)
        : base(commands, String.Empty)
    { }
    
    public RootTpLinkCommand() 
        : this(new InternalTpLinkCommand[]
               {
                   new WlanCompositeTpLinkCommand(),
                   new RefreshTpLinkCommand(),
                   new CheckConnectionTpLinkCommand()
               }) 
    { }
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