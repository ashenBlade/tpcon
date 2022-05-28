using Router.TpLink.CommandCreator;
using Router.TpLink.CommandCreator.Lan;
using Router.TpLink.CommandCreator.Root;
using Router.TpLink.CommandCreator.Wlan;

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