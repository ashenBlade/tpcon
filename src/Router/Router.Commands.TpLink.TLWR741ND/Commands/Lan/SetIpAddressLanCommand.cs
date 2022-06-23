using System.Net;
using Router.Commands.TpLink.Configurators.Lan;

namespace Router.Commands.TpLink.TLWR741ND.Commands.Lan;

public class SetIpAddressLanCommand : LanCommand
{
    private readonly IPAddress _address;

    public SetIpAddressLanCommand(ILanConfigurator configurator, IPAddress address) : base(configurator)
    {
        _address = address;
    }

    public override Task ExecuteAsync()
    {
        return Lan.SetIpAsync(_address);
    }
}