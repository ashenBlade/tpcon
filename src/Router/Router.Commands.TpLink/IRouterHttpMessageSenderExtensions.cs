using JsTypes;

namespace Router.Commands.TpLink;

public static class RouterHttpMessageSenderExtensions
{
    public static Task SendMessageAsync(this IRouterHttpMessageSender sender, string path, IEnumerable<KeyValuePair<string, string>>? query = null, HttpMethod? method = null) =>
        sender.SendMessageAsync(new RouterHttpMessage(path, query, method));
    
    public static Task<List<JsVariable>> SendMessageAndParseAsync(this IRouterHttpMessageSender sender, string path, IEnumerable<KeyValuePair<string, string>>? query = null, HttpMethod? method = null) =>
        sender.SendMessageAndParseAsync(new RouterHttpMessage(path, query, method));

    public static Task SendMessageAndSaveAsync(this IRouterHttpMessageSender sender, string path, IEnumerable<KeyValuePair<string, string>>? query = null, HttpMethod? method = null) =>
        sender.SendMessageAsync(new RouterHttpMessage(path,
                                                      query?.Concat(new[]
                                                                    {
                                                                        new KeyValuePair<string, string>("Save", "Save")
                                                                    }), 
                                                      method));
}