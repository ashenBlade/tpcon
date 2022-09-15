using Router.Commands.TpLink.CommandFactory;
using Router.Commands.TpLink.Configurators.Lan;

namespace Router.Commands.TpLink.TLWR741ND.CommandFactory.Lan;

internal class LanCompositeCommandFactory : CompositeTpLinkCommandFactory
{
    private static IEnumerable<KeyValuePair<CommandFactoryInfo, Func<BaseTpLinkCommandFactory>>> GetLanCommands(
        ILanConfigurator lan) =>
        new[]
        {
            Command("status", "Получить статус локальной сети",
                    () => new GetLanStatusCommandFactory(lan)),
            Command("ip", "Установить новый локальный адрес роутера. "
                        + "IP адрес задается в виде 4 чисел в диапазоне 0-255, разделенных точками. "
                        + "Пример: 192.168.0.1",
                    () => new SetIpAddressLanCommandFactory(lan)),
            Command("mask", "Изменить маску сети локальной сети. "
                          + "Маска задается в виде Wildcard. "
                          + "Например: 255.255.240.0",
                    () => new SetSubnetMaskLanCommandFactory(lan))
        };

    public LanCompositeCommandFactory(ILanConfigurator lan)
        : base(GetLanCommands(lan))
    {
    }
}