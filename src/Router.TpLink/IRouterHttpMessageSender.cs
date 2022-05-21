using JsTypes;
using Router.Domain;

namespace Router.TpLink;

public interface IRouterHttpMessageSender
{
    Task<List<JsVariable>> SendMessageAndParseAsync(RouterHttpMessage message);
    Task SendMessageAsync(RouterHttpMessage message);
}