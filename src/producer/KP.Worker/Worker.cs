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
            BootstrapServers = _kafkaOptions.BootstrapServers,
            Partitioner = Partitioner.Consistent
        };

        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            
            using (var producer = new ProducerBuilder<string, string>(config).Build())
            {
                _logger.LogInformation("Create producer");

                var weatherForecast = WeatherGenerator.Generate();
                
                _logger.LogInformation($"Start sending to topic: {_kafkaOptions.Topic}");
                
                await producer.ProduceAsync(_kafkaOptions.Topic, new Message<string, string>
                {
                    Key = weatherForecast.GetHashCode().ToString(),
                    Value = JsonSerializer.Serialize(weatherForecast)
                }, stoppingToken);
                
                _logger.LogInformation($"Finish sending to topic: {_kafkaOptions.Topic}");
            }

            await Task.Delay(1_000, stoppingToken);
        }
    }
}