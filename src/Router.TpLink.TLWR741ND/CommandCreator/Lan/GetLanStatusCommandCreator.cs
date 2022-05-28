using Router.Commands;
using Router.TpLink.Commands;

namespace Router.TpLink.CommandCreator.Lan;

internal class GetLanStatusCommandCreator : SingleTpLinkCommandCreator
{
    public GetLanStatusCommandCreator() : base("status")
    { }

    public override IRouterCommand CreateRouterCommand(RouterCommandContext context)
    {
        return new TpLinkGetLanStatusCommand(context.Router, context.OutputFormatter, Console.Out);
    }
}