using Router.TpLink.CommandCreators;
using Router.TpLink.TLWR741ND.CommandCreators.Lan;
using Router.TpLink.TLWR741ND.CommandCreators.Root;
using Router.TpLink.TLWR741ND.CommandCreators.Wlan;

namespace Router.TpLink.TLWR741ND;


public class TLWR741NDTpLinkCommandFactory : TpLinkCommandFactory
{
    public static IEnumerable<TpLinkCommandCreator> DefaultCommands
        => new TpLinkCommandCreator[]
           {
               new CheckConnectionTpLinkCommandCreator(), 
               new RefreshTpLinkCommandCreator(),
               new WlanCompositeCommandCreator(), 
               new LanCompositeCommandCreator()
           };
    
    public TLWR741NDTpLinkCommandFactory()
        : base(new TLWR741NDTpLinkRouterFactory(), DefaultCommands)
    { }
}