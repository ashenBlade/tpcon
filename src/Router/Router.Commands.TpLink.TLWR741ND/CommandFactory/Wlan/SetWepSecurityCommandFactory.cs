using Router.Commands.CommandLine.Exceptions;
using Router.Commands.TpLink.Configurators.Wlan;
using Router.Commands.TpLink.TLWR741ND.Commands.Wlan;
using Router.Utils.Security;

namespace Router.Commands.TpLink.TLWR741ND.CommandFactory.Wlan;

public class SetWepSecurityCommandFactory : WlanSingleCommandFactory
{
    private const string KeyFormat = "key-format";
    private const string Selected = "selected";

    public SetWepSecurityCommandFactory(IWlanConfigurator wlan)
        : base(wlan)
    {
    }

    public override IRouterCommand CreateRouterCommand(RouterCommandContext context)
    {
        var type = context.Arguments.TryGetValue("type", out var typeString)
                       ? typeString.ToLower() switch
                         {
                             "auto"                        => WepType.Automatic,
                             "open-system" or "opensystem" => WepType.OpenSystem,
                             "shared-key" or "sharedkey"   => WepType.SharedKey,
                             _ => throw new IncorrectArgumentValueException("type", typeString, context.Command,
                                                                            "Wep system type must be \"auto\" or \"open-system\" or \"shared-key\"")
                         }
                       : WepType.Automatic;
        var format = context.Arguments.TryGetValue(KeyFormat, out var formatString)
                         ? formatString.ToLower() switch
                           {
                               "hex"   => WepKeyFormat.Hex,
                               "ascii" => WepKeyFormat.ASCII,
                               _ => throw new IncorrectArgumentValueException(KeyFormat, formatString, context.Command,
                                                                              "Wep key format must be \"hex\" or \"ascii\"")
                           }
                         : WepKeyFormat.Hex;
        var key1 = GetWepKeyNumber(context, 1);
        var key2 = GetWepKeyNumber(context, 2);
        var key3 = GetWepKeyNumber(context, 3);
        var key4 = GetWepKeyNumber(context, 4);
        var selected = context.Arguments.TryGetValue(Selected, out var selectedString)
                           ? selectedString switch
                             {
                                 "1" => key1,
                                 "2" => key2,
                                 "3" => key3,
                                 "4" => key4,
                                 _ => throw new IncorrectArgumentValueException(Selected, selectedString,
                                                                                context.Command,
                                                                                "Selected key must be between 1 and 4")
                             }
                           : throw new ArgumentValueExpectedException(Selected, context.Command,
                                                                      "Specify key to use by \"--selected\" argument");
        var security = new WepSecurity(new[] {key1, key2, key3, key4}, selected, type, format);
        return new TpLinkSetWepSecurityCommand(Wlan, security);
    }

    private static WepKey GetWepKeyNumber(RouterCommandContext context, int number)
    {
        var arguments = context.Arguments;
        var key = arguments.TryGetValue($"key{number}", out var keyString)
                      ? keyString
                      : string.Empty;
        var encryption = key is ""
                             ? WepKeyEncryption.Disabled
                             : arguments.TryGetValue($"length{number}", out var lengthString)
                                 ? lengthString.ToLower() switch
                                   {
                                       "disabled"                    => WepKeyEncryption.Disabled,
                                       "bit64" or "64" or "64bit"    => WepKeyEncryption.Bit64,
                                       "bit128" or "128" or "128bit" => WepKeyEncryption.Bit128,
                                       "bit152" or "152" or "152bit" => WepKeyEncryption.Bit152,
                                       _ => throw new IncorrectArgumentValueException($"length{number}", lengthString,
                                                                                      context.Command,
                                                                                      "Wep key encryption must be \"disabled\" or \"bit64\" or \"bit128\" or \"bit152\"")
                                   }
                                 : throw new ArgumentValueExpectedException($"length{number}", context.Command,
                                                                            "Wep key encryption must be provided");
        return new WepKey(encryption, key);
    }
}