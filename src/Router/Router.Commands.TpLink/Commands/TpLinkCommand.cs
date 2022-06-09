using Router.Commands.TpLink.Configurators;

namespace Router.Commands.TpLink.Commands;

public abstract class TpLinkCommand<TConfigurator> : IRouterCommand
    where TConfigurator: IConfigurator
{
    public TConfigurator Configurator { get; }

    protected TpLinkCommand(TConfigurator configurator)
    {
        ArgumentNullException.ThrowIfNull(configurator);
        Configurator = configurator;
    }
    
    public abstract Task ExecuteAsync();
}