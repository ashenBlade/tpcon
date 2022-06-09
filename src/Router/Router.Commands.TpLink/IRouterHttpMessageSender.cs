using JsTypes;

namespace Router.Commands.TpLink;

public interface IRouterHttpMessageSender
{
    Task<List<JsVariable>> SendMessageAndParseAsync(RouterHttpMessage message);
    Task SendMessageAsync(RouterHttpMessage message);
}