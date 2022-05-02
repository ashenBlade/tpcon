namespace Router.Commands;

public interface IRouterCommandFactoryFactory
{
    public IRouterCommandFactory CreateRouterCommandFactory(RouterContext context);
}