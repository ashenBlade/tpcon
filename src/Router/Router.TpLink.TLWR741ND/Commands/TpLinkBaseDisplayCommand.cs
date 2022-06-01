using Router.Commands;
using Router.TpLink.TLWR741ND.Commands.DisplayStatus;

namespace Router.TpLink.TLWR741ND.Commands;

public abstract class TpLinkBaseDisplayCommand : TpLinkBaseCommand
{
    protected TextWriter Writer { get; }
    protected IOutputFormatter Formatter { get; }

    public TpLinkBaseDisplayCommand(TpLinkRouter router, TextWriter writer, IOutputFormatter formatter) : base(router)
    {
        Writer = writer;
        Formatter = formatter;
    }

    protected abstract Task<BaseDisplayStatus> GetStatusAsync();
    
    public sealed override async Task ExecuteAsync()
    {
        var status = await GetStatusAsync();
        var formatted = Formatter.Format(status);
        await Writer.WriteLineAsync(formatted);
    }
}