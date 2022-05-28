using JsTypes;

namespace Router.TpLink;

public interface IRouterHttpMessageSender
{
    Task<List<JsVariable>> SendMessageAndParseAsync(RouterHttpMessage message);
    Task SendMessageAsync(RouterHttpMessage message);
}