using System.ComponentModel;

namespace Router.Commands.TpLink.TLWR741ND.Commands.DisplayStatus.Router;

public class HealthCheckDisplayStatus : TpLink.Commands.DisplayStatus
{
    [DisplayName("Healthy")]
    public bool Healthy { get; }

    public HealthCheckDisplayStatus(bool healthy)
    {
        Healthy = healthy;
    }
}