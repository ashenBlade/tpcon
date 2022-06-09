using Router.Commands.CommandLine.Exceptions;
using Router.Commands.TpLink.Configurators.Wlan;
using Router.Commands.TpLink.TLWR741ND.Commands.Wlan;
using Router.Utils.Security;

namespace Router.Commands.TpLink.TLWR741ND.CommandFactory.Wlan;

public class SetPersonalSecurityCommandFactory: WlanSingleCommandFactory
{
    public SetPersonalSecurityCommandFactory(IWlanConfigurator wlan) : base(wlan, "personal")
    { }

    public override IRouterCommand CreateRouterCommand(RouterCommandContext context)
    {
        var interval = context.Arguments.TryGetValue("interval", out var intervalString)
                           ? int.TryParse(intervalString, out var i)
                                 ? i
                                 : throw new IncorrectArgumentValueException("interval", intervalString, "integer",
                                                                             context.Command.ToArray())
                           : 0;
        var password = context.Arguments.TryGetValue("password", out var passwordString)
                           ? passwordString
                           : context.CurrentCommand 
                          ?? throw new ArgumentValueExpectedException("password", context.Command.ToArray(), "Password must be provided after \"personal\" word or in \"--password\" argument");
        var encryption = context.Arguments.TryGetValue("encryption", out var encString)
                             ? encString.ToLower() switch
                               {
                                   "auto" => EncryptionType.Auto,
                                   "aes"  => EncryptionType.AES,
                                   "tkip" => EncryptionType.TKIP,
                                   _ => throw new IncorrectArgumentValueException("encryption", encString,
                                                                                  "auto | aes | tkip",
                                                                                  context.Command.ToArray(),
                                                                                  @"Supported encryption types are ""auto"" or ""aes"" or ""tkip""")
                               }
                             : EncryptionType.Auto;
        var version = context.Arguments.TryGetValue("version", out var versionString)
                          ? versionString.ToLower() switch
                            {
                                "auto" => SecurityVersion.Automatic,
                                "wpa"  => SecurityVersion.WPA,
                                "wpa2" => SecurityVersion.WPA2,
                                _ => throw new IncorrectArgumentValueException("version", versionString,
                                                                               "auto | wpa | wpa2",
                                                                               context.Command.ToArray(),
                                                                               @"Supported security versions are ""auto"" or ""wpa"" or ""wpa2""")
                            }
                          : SecurityVersion.Automatic;
        var personal = new PersonalSecurity(password, encryption, version, interval);
        return new TpLinkSetPersonalSecurityCommand(Wlan, personal);
    }
}