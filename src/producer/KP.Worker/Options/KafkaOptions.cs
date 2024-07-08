﻿namespace KP.Worker.Options;

public class KafkaOptions
{
    public string BootstrapServers { get; set; } = default!;

    public string Topic { get; set; } = default!;

    public int WaitInMilliseconds { get; set; } = 5;
}