namespace Router.Domain;

public static class RouterParametersExtensions
{
    public static Uri GetUriAddress(this RouterParameters parameters)
    {
        return new Uri($"http://{parameters.Address}");
    }
}