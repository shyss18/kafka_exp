using KC.Worker;
using KC.Worker.Options;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddOptions<KafkaOptions>()
            .BindConfiguration(nameof(KafkaOptions));

        services.AddHostedService<Worker>();
    })
    .Build();

await host.RunAsync();