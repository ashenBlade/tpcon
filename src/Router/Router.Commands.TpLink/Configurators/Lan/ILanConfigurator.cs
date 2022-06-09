using Router.Domain.Lan;

namespace Router.Commands.TpLink.Configurators.Lan;

public interface ILanConfigurator: IConfigurator
{
    Task<LanParameters> GetStatusAsync();
}