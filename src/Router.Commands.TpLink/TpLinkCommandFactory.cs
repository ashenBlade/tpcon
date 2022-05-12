using JsUtils.Implementation;
using Router.Commands.Commands;
using Router.Commands.TpLink.CommandFactories;
using Router.Commands.TpLink.CommandFactories.Wlan;
using Router.Commands.TpLink.Commands;
using Router.Commands.TpLink.Routers;
using Router.Domain;

namespace Router.Commands.TpLink;

public class TpLinkCommandFactory : IRouterCommandFactory
{
    private static IEnumerable<InternalTpLinkCommandCreator> DefaultCommands => new InternalTpLinkCommandCreator[]
                                                                                {
                                                                                    new CheckConnectionTpLinkCommandCreator(),
                                                                                    new RefreshTpLinkCommandCreator(),
                                                                                    new WlanCompositeTpLinkCommandCreator(),
                                                                                };
    public HttpClient Client { get; }
    private IEnumerable<InternalTpLinkCommandCreator> Factories { get; }

    public TpLinkCommandFactory(HttpClient client)
    : this(client, null)
    { }
    
    internal TpLinkCommandFactory(HttpClient client, IEnumerable<InternalTpLinkCommandCreator>? factories)
    {
        ArgumentNullException.ThrowIfNull(client);
        Factories = factories ?? DefaultCommands;
        Client = client;
    }
    
    public IRouterCommand CreateRouterCommand(CommandLineContext context)
    {
        var router = new TLWR741NDTpLinkRouter(new TpLinkRouterHttpMessageSender(new HtmlScriptVariableExtractor(new HtmlScriptExtractor(), new ScriptVariableExtractor(new Tokenizer())), Client, context.RouterParameters));
        var routerContext = new RouterCommandContext(router, context.Command, context.Arguments);
        return new RootTpLinkCommandCreator(DefaultCommands)
           .CreateRouterCommand(routerContext);
    }
}