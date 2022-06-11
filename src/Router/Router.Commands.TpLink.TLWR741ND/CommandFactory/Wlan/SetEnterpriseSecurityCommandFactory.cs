using System.Net;
using Router.Commands.CommandLine.Exceptions;
using Router.Commands.TpLink.Configurators.Wlan;
using Router.Commands.TpLink.TLWR741ND.Commands.Wlan;
using Router.Utils.Security;

namespace Router.Commands.TpLink.TLWR741ND.CommandFactory.Wlan;

public class SetEnterpriseSecurityCommandFactory : WlanSingleCommandFactory
{
    private const string Version = "version";
    private const string Encryption = "encryption";
    private const string RadiusIP = "radius-ip";
    private const string RadiusPort = "radius-port";
    private const string RadiusPassword = "radius-password";
    private const string GroupKeyUpdateInterval = "group-key-update-interval";
    private const int DefaultUpdateInterval = 0; // No update

    public SetEnterpriseSecurityCommandFactory(IWlanConfigurator wlan)
        : base(wlan, "enterprise")
    {
    }

    public override IRouterCommand CreateRouterCommand(RouterCommandContext context)
    {
        var version = context.Arguments.TryGetValue(Version, out var ver)
                          ? ver.ToLower() switch
                            {
                                "auto" => SecurityVersion.Automatic,
                                "wpa"  => SecurityVersion.WPA,
                                "wpa2" => SecurityVersion.WPA2,
                                _ => throw new IncorrectArgumentValueException(Version, ver, "auto | wpa | wpa2",
                                                                               context.Command.ToArray()),
                            }
                          : SecurityVersion.Automatic;
        var encryption = context.Arguments.TryGetValue(Encryption, out var enc)
                             ? enc.ToLower() switch
                               {
                                   "auto" => EncryptionType.Auto,
                                   "tkip" => EncryptionType.TKIP,
                                   "aes"  => EncryptionType.AES,
                                   _ => throw new IncorrectArgumentValueException(Encryption, enc,
                                                                                  "auto | tkip | aes",
                                                                                  context.Command.ToArray())
                               }
                             : EncryptionType.Auto;
        var ip = context.Arguments.TryGetValue(RadiusIP, out var radiusIp)
                     ? IPAddress.TryParse(radiusIp, out var parsedIp)
                           ? parsedIp
                           : throw new IncorrectArgumentValueException(RadiusIP, radiusIp, context.Command.ToArray(),
                                                                       "Incorrect ip address representation for RADIUS server")
                     : throw new ArgumentValueExpectedException(RadiusIP, context.Command.ToArray(),
                                                                "RADIUS server ip address must be provided");
        var port = context.Arguments.TryGetValue(RadiusPort, out var radiusPort)
                       ? int.TryParse(radiusPort, out var p)
                             ? p
                             : throw new IncorrectArgumentValueException(RadiusPort, radiusPort,
                                                                         context.Command.ToArray(),
                                                                         "RADIUS server port must be integer")
                       : RadiusServer.DefaultPort;
        var password = context.Arguments.TryGetValue(RadiusPassword, out var radiusPassword)
                           ? radiusPassword
                           : throw new ArgumentValueExpectedException(RadiusPassword, context.Command.ToArray(),
                                                                      "RADIUS server password must be provided");
        var interval = context.Arguments.TryGetValue(GroupKeyUpdateInterval, out var groupKeyUpdateInterval)
                           ? int.TryParse(groupKeyUpdateInterval, out var groupKey)
                                 ? groupKey
                                 : throw new IncorrectArgumentValueException(GroupKeyUpdateInterval,
                                                                             groupKeyUpdateInterval,
                                                                             context.Command.ToArray(),
                                                                             "Group key update interval must be integer")
                           : DefaultUpdateInterval;
        var radius = new RadiusServer(password, ip, port);
        var security = new EnterpriseSecurity(radius, version, encryption, interval);
        return new TpLinkSetEnterpriseSecurityCommand(Wlan, security);
    }
}