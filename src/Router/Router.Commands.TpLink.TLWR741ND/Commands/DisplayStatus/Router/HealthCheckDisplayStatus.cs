using System.ComponentModel;

namespace Router.Commands.TpLink.TLWR741ND.Commands.DisplayStatus.Router;

public class HealthCheckDisplayStatus : TpLink.Commands.DisplayStatus
{
    [DisplayName("Здоров")]
    public bool Healthy { get; }

    public HealthCheckDisplayStatus(bool healthy)
    {
        Healthy = healthy;
    }
}