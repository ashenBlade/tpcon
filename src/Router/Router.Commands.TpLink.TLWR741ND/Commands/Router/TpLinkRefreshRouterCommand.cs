using Router.Commands.TpLink.Commands;
using Router.Commands.TpLink.Configurators.Router;

namespace Router.Commands.TpLink.TLWR741ND.Commands.Router;

public class TpLinkRefreshRouterCommand : TpLinkCommand<IRouterConfigurator>
{
    public TpLinkRefreshRouterCommand(IRouterConfigurator configurator) 
        : base(configurator)
    { }

    public override Task ExecuteAsync()
    {
        return Configurator.RefreshAsync();
    }
}