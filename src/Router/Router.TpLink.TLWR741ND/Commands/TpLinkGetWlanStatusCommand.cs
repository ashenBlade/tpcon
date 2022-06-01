using Router.Commands;
using Router.TpLink.TLWR741ND.Commands.DisplayStatus;
using Router.TpLink.TLWR741ND.Utils;

namespace Router.TpLink.TLWR741ND.Commands;

public class TpLinkGetWlanStatusCommand : TpLinkBaseDisplayCommand
{
    public TpLinkGetWlanStatusCommand(TextWriter output, TpLinkRouter router, IOutputFormatter formatter) 
        : base(router, output, formatter)
    { }

    protected override async Task<BaseDisplayStatus> GetStatusAsync()
    {
        return ( await Router.Wlan.GetStatusAsync() ).ToDisplayStatus();
    }
}