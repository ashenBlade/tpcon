using Router.Commands.TpLink.Commands;
using Router.Commands.TpLink.Configurators.Router;
using Router.Commands.TpLink.TLWR741ND.Commands.DisplayStatus.Router;

namespace Router.Commands.TpLink.TLWR741ND.Commands.Router;

public class TpLinkHealthCheckCommand : TpLinkQueryCommand<IRouterConfigurator>
{
    public TpLinkHealthCheckCommand(IRouterConfigurator configurator, TextWriter output, IOutputFormatter formatter) : base(configurator, output, formatter)
    {
    }

    protected override async Task<TpLink.Commands.DisplayStatus> GetDisplayStatusAsync()
    {
        return new HealthCheckDisplayStatus(await Configurator.CheckConnectionAsync());
    }
}