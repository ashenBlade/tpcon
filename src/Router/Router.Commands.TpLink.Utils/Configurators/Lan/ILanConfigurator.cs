using Router.Domain.Properties;

namespace Router.Commands.TpLink.Utils;

public interface ILanConfigurator: IConfigurator
{
    Task<LanParameters> GetStatusAsync();
}