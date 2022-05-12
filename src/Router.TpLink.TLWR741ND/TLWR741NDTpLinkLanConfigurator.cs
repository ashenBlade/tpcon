using Router.Domain.RouterProperties;

namespace Router.TpLink.TLWR741ND;

public class TLWR741NDTpLinkLanConfigurator : ILanConfigurator
{
    private readonly IRouterHttpMessageSender _messageSender;

    public TLWR741NDTpLinkLanConfigurator(IRouterHttpMessageSender messageSender)
    {
        _messageSender = messageSender;
    }
    
    public Task<LanParameters> GetStatusAsync()
    {
        throw new NotImplementedException();
    }
}