using System.Diagnostics;
using System.Runtime.CompilerServices;
using Router.Domain.Properties;

namespace Router.Commands.TpLink.Utils;

public abstract class BaseLanConfigurator : BaseConfigurator, ILanConfigurator
{
    protected BaseLanConfigurator(IRouterHttpMessageSender messageSender) 
        : base(messageSender)
    { }

    public abstract Task<LanParameters> GetStatusAsync();
}