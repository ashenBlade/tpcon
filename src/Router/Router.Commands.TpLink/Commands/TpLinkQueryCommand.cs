using Router.Commands.TpLink.Configurators;

namespace Router.Commands.TpLink.Commands;

public abstract class TpLinkQueryCommand<TConfigurator> : TpLinkCommand<TConfigurator>, IQueryRouterCommand
    where TConfigurator: IConfigurator
{
    protected TextWriter Output { get; }
    protected IOutputFormatter Formatter { get; }

    protected TpLinkQueryCommand(TConfigurator configurator, 
                                 TextWriter output,
                                 IOutputFormatter formatter) 
        : base(configurator)
    {
        ArgumentNullException.ThrowIfNull(output);
        ArgumentNullException.ThrowIfNull(formatter);
        Output = output;
        Formatter = formatter;
    }

    protected abstract Task<DisplayStatus> GetDisplayStatusAsync();
    
    public override async Task ExecuteAsync()
    {
        var status = await GetDisplayStatusAsync();
        var formatted = Formatter.Format(status);
        await Output.WriteLineAsync(formatted);
    }
}