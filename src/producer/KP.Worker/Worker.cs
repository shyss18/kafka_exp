using System.Text.Json;
using Confluent.Kafka;
using KP.Worker.Generator;
using KP.Worker.Options;
using Microsoft.Extensions.Options;

namespace KP.Worker;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly KafkaOptions _kafkaOptions;

    public Worker(
        ILogger<Worker> logger,
        IOptions<KafkaOptions> kafkaOptions)
    {
        _logger = logger;
        _kafkaOptions = kafkaOptions.Value;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var config = new ProducerConfig
        {
            BootstrapServers = _kafkaOptions.BootstrapServers
        };

        using var producer = new ProducerBuilder<Null, string>(config).Build();

        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

            var weatherForecast = WeatherGenerator.Generate();

            _logger.LogInformation($"Start sending to topic: {_kafkaOptions.Topic}");

            await producer.ProduceAsync(_kafkaOptions.Topic, new Message<Null, string>
            {
                Value = JsonSerializer.Serialize(weatherForecast)
            }, stoppingToken);

            _logger.LogInformation($"Finish sending to topic: {_kafkaOptions.Topic}");

            _logger.LogInformation($"Wait in milliseconds: {_kafkaOptions.WaitInMilliseconds}");
            
            await Task.Delay(_kafkaOptions.WaitInMilliseconds, stoppingToken);
        }
    }
}