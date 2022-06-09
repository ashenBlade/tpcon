using Router.Domain.Lan;

namespace Router.Commands.TpLink.Configurators.Lan;

public abstract class BaseLanConfigurator : BaseConfigurator, ILanConfigurator
{
    protected BaseLanConfigurator(IRouterHttpMessageSender messageSender) 
        : base(messageSender)
    { }

    public abstract Task<LanParameters> GetStatusAsync();
}