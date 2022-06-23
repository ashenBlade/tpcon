using Router.Commands.TpLink.Configurators.Lan;
using Router.Domain.Lan;

namespace Router.Commands.TpLink.TLWR741ND.Commands.Lan;

public class SetSubnetMaskLanCommand : LanCommand
{
    private readonly SubnetMask _mask;

    public SetSubnetMaskLanCommand(ILanConfigurator configurator, SubnetMask mask) : base(configurator)
    {
        _mask = mask;
    }

    public override Task ExecuteAsync()
    {
        return Lan.SetSubnetMaskAsync(_mask);
    }
}