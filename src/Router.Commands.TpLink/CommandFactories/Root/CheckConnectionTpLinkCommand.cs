using Router.Commands.TpLink.Commands;
using Router.Commands.TpLink.Routers;

namespace Router.Commands.TpLink.CommandFactories;

internal class CheckConnectionTpLinkCommand : SingleCommandTpLinkCommand
{
    public CheckConnectionTpLinkCommand() : base("health") 
    { }
    public override IRouterCommand CreateRouterCommand(CommandContext context)
    {
        var router = new TLWR741NDTpLinkRouter(context.RouterParameters);
        return new TpLinkHealthCheckCommand(router);
    }

    public override void WriteHelpTo(TextWriter writer)
    {
        throw new NotImplementedException();
    }
}