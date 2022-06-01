using Router.Commands;
using Router.TpLink.TLWR741ND.Commands.DisplayStatus;

namespace Router.TpLink.TLWR741ND.Commands;

public class TpLinkGetLanStatusCommand : TpLinkBaseDisplayCommand
{
    public TpLinkGetLanStatusCommand(TpLinkRouter router, TextWriter writer, IOutputFormatter formatter) 
        : base(router, writer, formatter)
    { }

    protected override async Task<BaseDisplayStatus> GetStatusAsync()
    {
        var lan = await Router.Lan.GetStatusAsync();
        return new LanDisplayStatus(lan.IpAddress, lan.MacAddress, lan.SubnetMask);
    }
}