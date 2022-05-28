using Router.TpLink.CommandCreators;
using Router.TpLink.TLWR741ND.CommandCreator.Lan;
using Router.TpLink.TLWR741ND.CommandCreator.Root;
using Router.TpLink.TLWR741ND.CommandCreator.Wlan;

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