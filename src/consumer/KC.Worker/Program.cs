using KC.Worker;
using KC.Worker.Options;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration(configure =>
    {
        configure.AddJsonFile("appsettings.json", false);
        configure.AddJsonFile("appsettings.Development.json", false);
    })
    .ConfigureServices(services =>
    {
        services.AddOptions<KafkaOptions>()
            .BindConfiguration(nameof(KafkaOptions));

        services.AddHostedService<Worker>();
    })
    .Build();

await host.RunAsync();