namespace Router.Domain;

public class Rate
{
    public Rate(int speed, NetworkSpeedMeasurement measurement)
    {
        if (speed < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(speed), "Speed can not be negative");
        }

        if (measurement is NetworkSpeedMeasurement.None)
        {
            throw new ArgumentOutOfRangeException(nameof(measurement), "Speed measurement must be specified");
        }
        Speed = speed;
        Measurement = measurement;
    }

    public int Speed { get; }
    public NetworkSpeedMeasurement Measurement { get; }
}