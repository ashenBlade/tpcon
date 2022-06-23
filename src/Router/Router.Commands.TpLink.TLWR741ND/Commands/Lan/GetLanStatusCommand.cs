using Router.Commands.TpLink.Commands;
using Router.Commands.TpLink.Configurators.Lan;
using Router.Commands.TpLink.TLWR741ND.Commands.DisplayStatus;

namespace Router.Commands.TpLink.TLWR741ND.Commands.Lan;

public class GetLanStatusCommand : LanQueryCommand
{
    public GetLanStatusCommand(ILanConfigurator configurator, TextWriter writer, IOutputFormatter formatter)
        : base(configurator, writer, formatter)
    {
    }


    protected override async Task<TpLink.Commands.DisplayStatus> GetDisplayStatusAsync()
    {
        var lan = await Configurator.GetStatusAsync();
        return new LanDisplayStatus(lan);
    }
}