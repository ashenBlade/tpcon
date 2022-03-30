namespace Router.Domain;

public static class RouterParametersExtensions
{
    public static Uri Address(this RouterParameters parameters)
    {
        return new Uri($"http://{parameters.IpAddress}");
    }
}