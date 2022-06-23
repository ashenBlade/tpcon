using Router.Commands.TpLink.Commands;
using Router.Commands.TpLink.Configurators.Wlan;

namespace Router.Commands.TpLink.TLWR741ND.Commands.Wlan;

public abstract class WlanQueryCommand : TpLinkQueryCommand<IWlanConfigurator>
{
    protected IWlanConfigurator Wlan => Configurator;

    protected WlanQueryCommand(IWlanConfigurator configurator, TextWriter output, IOutputFormatter formatter)
        : base(configurator, output, formatter)
    {
    }
}