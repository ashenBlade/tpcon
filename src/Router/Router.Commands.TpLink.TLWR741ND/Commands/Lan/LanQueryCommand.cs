using Router.Commands.TpLink.Commands;
using Router.Commands.TpLink.Configurators.Lan;
using Router.Commands.TpLink.TLWR741ND.Configurators;

namespace Router.Commands.TpLink.TLWR741ND.Commands.Lan;

public abstract class LanQueryCommand : TpLinkQueryCommand<ILanConfigurator>
{
    protected ILanConfigurator Lan => Configurator;

    protected LanQueryCommand(ILanConfigurator configurator, TextWriter output, IOutputFormatter formatter)
        : base(configurator, output, formatter)
    {
    }
}