using Router.Commands;
using Router.TpLink.CommandCreators;
using Router.TpLink.TLWR741ND.Commands;

namespace Router.TpLink.TLWR741ND.CommandCreators.Lan;

internal class GetLanStatusCommandCreator : SingleTpLinkCommandCreator
{
    public GetLanStatusCommandCreator() : base("status")
    { }

    public override IRouterCommand CreateRouterCommand(RouterCommandContext context)
    {
        return new TpLinkGetLanStatusCommand(context.Router, context.OutputFormatter, Console.Out);
    }
}