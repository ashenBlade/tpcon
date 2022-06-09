using Router.Commands.TpLink.Commands;
using Router.Commands.TpLink.Configurators.Lan;
using Router.Commands.TpLink.TLWR741ND.Commands.DisplayStatus;

namespace Router.Commands.TpLink.TLWR741ND.Commands;

public class TpLinkGetLanStatusCommand : TpLinkQueryCommand<ILanConfigurator>
{
    public TpLinkGetLanStatusCommand(ILanConfigurator configurator, TextWriter writer, IOutputFormatter formatter) 
        : base(configurator, writer, formatter)
    { }


    protected override async Task<TpLink.Commands.DisplayStatus> GetDisplayStatusAsync()
    {
        var lan = await Configurator.GetStatusAsync();
        return new LanDisplayStatus(lan);
    }
}