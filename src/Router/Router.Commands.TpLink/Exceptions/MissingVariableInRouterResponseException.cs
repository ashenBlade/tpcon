using Router.Exceptions;

namespace Router.Commands.TpLink.Exceptions;

public class MissingVariableInRouterResponseException : InvalidRouterResponseException
{
    public MissingVariableInRouterResponseException(string missingVariableName,
                                                    string requestUri,
                                                    string? message = null)
        : base(message ?? $"В ответе отсутствует требуемая переменная \"{missingVariableName}\"")
    {
        MissingVariableName = missingVariableName;
        RequestUri = requestUri;
    }

    public string MissingVariableName { get; set; }
    public string RequestUri { get; set; }
}