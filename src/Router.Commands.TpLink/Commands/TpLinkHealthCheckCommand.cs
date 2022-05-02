using Router.Commands.Commands;
using Router.Domain;
using Router.Domain.Exceptions;

namespace Router.Commands.TpLink.Commands;

public class TpLinkHealthCheckCommand : HealthCheckCommand
{
    public TpLinkHealthCheckCommand(RouterParameters routerParameters) : base(routerParameters) { }
    public override async Task ExecuteAsync()
    {
        using var client = new HttpClient();
        try
        {
            using var msg = GetRequestMessageBase(string.Empty);
            using var response = await client.SendAsync(msg);
            Console.WriteLine($"OK");
        }
        catch (HttpRequestException)
        {
            throw new RouterUnreachableException(RouterParameters.GetAddress().ToString());
        }
    }
}