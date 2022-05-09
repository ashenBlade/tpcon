using Router.Commands.TpLink.Commands;
using Router.Commands.TpLink.Routers;

namespace Router.Commands.TpLink.CommandFactories.Wlan;

internal class GetWlanStatusCompositeTpLinkCommand : SingleCommandTpLinkCommand
{
    public GetWlanStatusCompositeTpLinkCommand() 
        : base("status") 
    { }
    public override IRouterCommand CreateRouterCommand(CommandContext context)
    {
        return new TpLinkGetWlanStatusCommand(Console.Out, new TLWR741NDTpLinkRouter(context.RouterParameters));
    }

    public override void WriteHelpTo(TextWriter writer)
    {
        throw new NotImplementedException();
    }
}