using Router.Commands.TpLink.Configurators.Wlan;

namespace Router.Commands.TpLink.TLWR741ND.Commands.Wlan;

public class TpLinkSetWlanSsidCommand : WlanTpLinkCommand
{
    private readonly string _ssid;

    public TpLinkSetWlanSsidCommand(IWlanConfigurator router, string ssid)
        : base(router)
    {
        _ssid = ssid;
    }

    public override Task ExecuteAsync()
    {
        return Wlan.SetSSIDAsync(_ssid);
    }
}