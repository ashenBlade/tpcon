using Router.Commands.TpLink.Commands;
using Router.Commands.TpLink.Configurators.Lan;
using Router.Commands.TpLink.TLWR741ND.Configurators;

namespace Router.Commands.TpLink.TLWR741ND.Commands.Lan;

public abstract class LanCommand : TpLinkCommand<ILanConfigurator>
{
    protected ILanConfigurator Lan => Configurator;

    protected LanCommand(ILanConfigurator configurator) : base(configurator)
    {
    }
}