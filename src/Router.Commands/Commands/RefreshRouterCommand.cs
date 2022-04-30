using Router.Domain;

namespace Router.Commands.Commands;

public class RefreshRouterCommand : RouterCommand
{
    public RefreshRouterCommand(RouterParameters routerParameters) 
        : base(routerParameters) 
    { }
    
    public override async Task ExecuteAsync()
    {
        using var client = new HttpClient();
        using var message = GetRequestMessageBase("/userRpm/SysRebootRpm.htm", "Reboot=Reboot");
        var response = await client.SendAsync(message);
        response.EnsureSuccessStatusCode();
    }
}