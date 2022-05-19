using Router.Commands;
using Router.TpLink.Commands;

namespace Router.TpLink.CommandFactories.Lan;

internal class GetLanStatusCommandFactory : SingleTpLinkCommandFactory
{
    public GetLanStatusCommandFactory() : base("status")
    { }

    public override IRouterCommand CreateRouterCommand(RouterCommandContext context)
    {
        return new TpLinkGetLanStatusCommand(context.Router, context.OutputFormatter, Console.Out);
    }
}