using Router.Commands;
using Router.Domain.Exceptions;
using Router.TpLink.TLWR741ND.Commands.DisplayStatus;
using Router.TpLink.TLWR741ND.Utils.Security;

namespace Router.TpLink.TLWR741ND.Commands;

public class TpLinkGetWlanSecurityStatusCommand : TpLinkBaseDisplayCommand
{
    public TpLinkGetWlanSecurityStatusCommand(TpLinkRouter router, TextWriter writer, IOutputFormatter formatter) 
        : base(router, writer, formatter)
    { }
    
    protected override async Task<BaseDisplayStatus> GetStatusAsync()
    {
        var status = await Router.Wlan.GetStatusAsync();
        var security = status.Security as CustomSecurity 
                    ?? throw new InvalidRouterResponseException();
        return security.ToDisplayStatus();
    }
}