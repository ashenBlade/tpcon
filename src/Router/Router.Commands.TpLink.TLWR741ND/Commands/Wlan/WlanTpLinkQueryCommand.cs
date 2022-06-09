using Router.Commands.TpLink.Commands;
using Router.Commands.TpLink.Configurators.Wlan;

namespace Router.Commands.TpLink.TLWR741ND.Commands.Wlan;

public abstract class WlanTpLinkQueryCommand : TpLinkQueryCommand<IWlanConfigurator>
{
    protected IWlanConfigurator Wlan => Configurator;
    
    protected WlanTpLinkQueryCommand(IWlanConfigurator configurator, TextWriter output, IOutputFormatter formatter) 
        : base(configurator, output, formatter)
    { }
}