namespace KP.Worker.Options;

public class KafkaOptions
{
    public string BootstrapServers { get; set; } = default!;

    public string Topic { get; set; } = default!;

    public int WaitInSeconds { get; set; } = 5;
}