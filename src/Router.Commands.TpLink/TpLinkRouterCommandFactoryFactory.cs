namespace Router.Commands.TpLink;

public class TpLinkRouterCommandFactoryFactory : IRouterCommandFactoryFactory
{
    public IRouterCommandFactory CreateRouterCommandFactory(RouterContext context)
    {
        return new TpLinkCommandFactory(context.RouterParameters);
    }
}