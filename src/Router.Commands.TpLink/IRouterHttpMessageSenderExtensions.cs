using JsTypes;

namespace Router.Commands.TpLink;

public static class IRouterHttpMessageSenderExtensions
{
    public static Task SendMessageAsync(this IRouterHttpMessageSender sender, string path) =>
        sender.SendMessageAsync(new RouterHttpMessage(path));
 
    public static Task<List<JsVariable>> SendMessageAndParseAsync(this IRouterHttpMessageSender sender, string path) =>
        sender.SendMessageAndParseAsync(new RouterHttpMessage(path));

}