using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Router.Commands.TpLink.TLWR741ND.Commands.DisplayStatus.Router;

public class HealthCheckDisplayStatus : TpLink.Commands.DisplayStatus
{
    [DisplayName("Подключение")]
    public bool Healthy { get; }

    public HealthCheckDisplayStatus(bool healthy)
    {
        Healthy = healthy;
    }
}