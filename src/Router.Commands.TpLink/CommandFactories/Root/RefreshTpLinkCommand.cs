using Router.Commands.TpLink.Commands;
using Router.Commands.TpLink.Routers;

namespace Router.Commands.TpLink.CommandFactories;

internal class RefreshTpLinkCommand : SingleCommandTpLinkCommand
{
    public RefreshTpLinkCommand() : base("refresh") { }
    public override IRouterCommand CreateRouterCommand(CommandContext context)
    {
        var router = new TLWR741NDTpLinkRouter(context.RouterParameters);
        return new TpLinkRefreshRouterCommand(router);
    }

    public override void WriteHelpTo(TextWriter writer)
    {
        throw new NotImplementedException();
    }
}