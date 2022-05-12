using JsTypes;

namespace Router.TpLink;

public static class IRouterHttpMessageSenderExtensions
{
    public static Task SendMessageAsync(this IRouterHttpMessageSender sender, string path, IEnumerable<KeyValuePair<string, string>>? query = null, HttpMethod? method = null) =>
        sender.SendMessageAsync(new RouterHttpMessage(path, query, method));
    
    public static Task<List<JsVariable>> SendMessageAndParseAsync(this IRouterHttpMessageSender sender, string path, IEnumerable<KeyValuePair<string, string>>? query = null, HttpMethod? method = null) =>
        sender.SendMessageAndParseAsync(new RouterHttpMessage(path, query, method));

}