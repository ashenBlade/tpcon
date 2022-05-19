using JsTypes;
using Router.TpLink.Status.Lan.Network;

namespace Router.TpLink.TLWR741ND.Status.Lan.NetworkCfg;

public class TLWR741NDLanNetworkPageStatus : LanNetworkPageStatus
{
    public TLWR741NDLanNetworkPageStatus(JsArray lanPara)
    {
        LanPara = lanPara;
    }

    public JsArray LanPara { get; }
}