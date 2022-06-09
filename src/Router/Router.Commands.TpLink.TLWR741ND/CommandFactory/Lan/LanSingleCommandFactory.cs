using Router.Commands.TpLink.CommandFactory;
using Router.Commands.TpLink.Configurators.Lan;

namespace Router.Commands.TpLink.TLWR741ND.CommandFactory.Lan;

public abstract class LanSingleCommandFactory : SingleTpLinkCommandFactory
{
    protected ILanConfigurator Lan { get; }

    public LanSingleCommandFactory(ILanConfigurator lan, string name) : base(name)
    {
        ArgumentNullException.ThrowIfNull(lan);
        Lan = lan;
    }
}