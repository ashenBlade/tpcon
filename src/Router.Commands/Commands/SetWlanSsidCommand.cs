using Router.Domain;

namespace Router.Commands.Commands;

public abstract class SetWlanSsidCommand : BaseRouterCommand
{
    public string Ssid { get; }

    protected SetWlanSsidCommand(RouterParameters routerParameters, string ssid) 
        : base(routerParameters)
    {
        Ssid = ssid;
    }
}