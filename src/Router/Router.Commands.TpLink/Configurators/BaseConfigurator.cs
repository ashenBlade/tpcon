using JsTypes;
using Router.Commands.TpLink.Exceptions;

namespace Router.Commands.TpLink.Configurators;

public abstract class BaseConfigurator : IConfigurator
{
    protected BaseConfigurator(IRouterHttpMessageSender messageSender)
    {
        MessageSender = messageSender;
    }

    protected IRouterHttpMessageSender MessageSender { get; }
    protected async Task<Dictionary<string, JsType>> GetPageVariablesAsync(string path) =>
        ( await MessageSender.SendMessageAndParseAsync(path) )
       .ToDictionary(v => v.Name, v => v.Value);
    
    protected static TJsType GetRequired<TJsType>(Dictionary<string, JsType> variables, string name, string path)
        where TJsType: JsType =>
        variables.TryGetValue(name, out var type)
            ? type as TJsType ?? throw new ExpectedVariableTypeMismatchException(name, typeof(TJsType), type.GetType())
            : throw new MissingVariableInRouterResponseException(name, path);


    protected static JsArray GetRequiredArray(Dictionary<string, JsType> variables, string name, string path) =>
        GetRequired<JsArray>(variables, name, path);
}