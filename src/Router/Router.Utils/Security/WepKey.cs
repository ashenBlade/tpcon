namespace Router.Utils.Security;

public struct WepKey
{
    public WepKey(WepKeyEncryption encryption, string key)
    {
        ArgumentNullException.ThrowIfNull(key);
        Encryption = encryption;
        Key = key;
    }

    public string Key { get; } = string.Empty;
    public WepKeyEncryption Encryption { get; } = WepKeyEncryption.Disabled;
}