namespace Router.Domain.RouterProperties;

public struct RouterStatistics
{
    public TimeSpan WorkTime { get; init; }
    public int BytesReceived { get; init; }
    public int BytesSend { get; init; }
    public int PacketsReceived { get; init; }
    public int PacketsSend { get; init; }
}