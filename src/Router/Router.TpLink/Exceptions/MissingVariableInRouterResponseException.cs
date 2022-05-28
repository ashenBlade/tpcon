using Router.Domain.Exceptions;

namespace Router.TpLink.Exceptions;

public class MissingVariableInRouterResponseException : InvalidRouterResponseException
{
    public MissingVariableInRouterResponseException(string missingVariableName, string requestUri, string? message = null) 
        : base(message ?? $"Missing \"{missingVariableName}\" variable in response from \"{requestUri}\"")
    {
        MissingVariableName = missingVariableName;
        RequestUri = requestUri;
    }

    public string MissingVariableName { get; set; }
    public string RequestUri { get; set; }
    
}