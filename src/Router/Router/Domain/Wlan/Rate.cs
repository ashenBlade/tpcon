namespace Router.Domain.Wlan;

public class Rate
{
    public Rate(int speed, NetworkSpeedMeasurement measurement)
    {
        if (speed < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(speed), speed, "Скорость сети не может быть отрицательной");
        }

        if (measurement is NetworkSpeedMeasurement.None)
        {
            throw new ArgumentOutOfRangeException(nameof(measurement), measurement, "Нужно указать тип скорости сети");
        }

        Speed = speed;
        Measurement = measurement;
    }

    public int Speed { get; }
    public NetworkSpeedMeasurement Measurement { get; }

    public override string ToString()
    {
        return $"{Speed} {Measurement}";
    }
}