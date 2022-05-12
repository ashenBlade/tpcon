using JsTypes;
using Router.Domain;

namespace Router.Commands.TpLink;

public interface IRouterHttpMessageSender
{
    public RouterParameters RouterParameters { get; set; }
    Task<List<JsVariable>> SendMessageAndParseAsync(RouterHttpMessage message);
    Task SendMessageAsync(RouterHttpMessage message);
}