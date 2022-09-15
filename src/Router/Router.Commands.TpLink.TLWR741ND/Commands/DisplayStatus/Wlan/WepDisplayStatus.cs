using System.ComponentModel;
using Router.Utils.Security;

namespace Router.Commands.TpLink.TLWR741ND.Commands.DisplayStatus.Wlan;

public class WepDisplayStatus : SecurityDisplayStatus
{
    public override string Name => "WEP";

    [DisplayName("Type")]
    public string Type { get; }

    [DisplayName("Key format")]
    public string Format { get; }

    [DisplayName("Key type")]
    public string KeyType { get; }

    [DisplayName("Key")]
    public string Key { get; }

    public WepDisplayStatus(WepSecurity security)
        : this(security.Format, security.Type, security.Selected)
    {
    }

    private WepDisplayStatus(WepKeyFormat format, WepType type, WepKey selected)
    {
        Type = type switch
               {
                   WepType.Automatic  => "Автоматический",
                   WepType.OpenSystem => "Открытая система",
                   WepType.SharedKey  => "Общий ключ",
                   _                  => throw new ArgumentOutOfRangeException(nameof(type))
               };
        Format = format switch
                 {
                     WepKeyFormat.Hex       => "Шестнадцатеричный",
                     WepKeyFormat.ASCII     => "ASCII",
                     WepKeyFormat.Undefined => "Неопределен",
                     _                      => throw new ArgumentOutOfRangeException(nameof(format))
                 };
        Key = selected.Key;
        KeyType = selected.Encryption switch
                  {
                      WepKeyEncryption.Bit64    => "64 bit",
                      WepKeyEncryption.Bit128   => "128 bit",
                      WepKeyEncryption.Bit152   => "152 bit",
                      WepKeyEncryption.Disabled => "Неопределен",
                      _                         => throw new ArgumentOutOfRangeException(nameof(selected.Encryption))
                  };
    }
}