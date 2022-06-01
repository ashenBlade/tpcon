using Router.TpLink.CommandCreators;
using Router.TpLink.TLWR741ND.Commands;

namespace Router.TpLink.TLWR741ND.CommandCreator.Wlan;

public class WlanSecurityCompositeCommandCreator : CompositeTpLinkCommandCreator
{
    public static IEnumerable<TpLinkCommandCreator> WlanSecurityCommands => new[] {new GetWlanSecurityStatusCommandCreator()};
    
    public WlanSecurityCompositeCommandCreator() 
        : base(WlanSecurityCommands, "security")
    { }
}