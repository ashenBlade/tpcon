using Router.Commands.TpLink.Configurators.Wlan;

namespace Router.Commands.TpLink.TLWR741ND.Commands.Wlan;

public abstract class TpLinkSetWirelessRadioCommand : WlanTpLinkCommand
{
    private readonly bool _enable;

    protected TpLinkSetWirelessRadioCommand(IWlanConfigurator configurator, bool enable) 
        : base(configurator)
    {
        _enable = enable;
    }
    public override async Task ExecuteAsync()
    {
        await ( _enable
                    ? Configurator.EnableAsync()
                    : Configurator.DisableAsync() );
    }
}