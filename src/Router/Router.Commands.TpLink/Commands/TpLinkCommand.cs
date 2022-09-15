using Router.Commands.TpLink.Configurators;

namespace Router.Commands.TpLink.Commands;

public abstract class TpLinkCommand : IRouterCommand
{
    public abstract Task ExecuteAsync();
}

public abstract class TpLinkCommand<TConfigurator> : TpLinkCommand
    where TConfigurator : IConfigurator
{
    public TConfigurator Configurator { get; }

    protected TpLinkCommand(TConfigurator configurator)
    {
        ArgumentNullException.ThrowIfNull(configurator);
        Configurator = configurator;
    }
}