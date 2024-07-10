using System.Text.Json;
using Confluent.Kafka;
using KP.Worker.Generator;
using KP.Worker.Options;
using KP.Worker.Partitioner;
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
            EnableIdempotence = true,
            AllowAutoCreateTopics = false,
        };

        using var producer = new ProducerBuilder<string, string>(config)
            .SetPartitioner(_kafkaOptions.Topic, (_, _, data, _) => CustomPartitioner.Partition(data, _logger))
            .Build();

        var index = 0;
        
        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

            var weatherForecast = WeatherGenerator.Generate().ToArray();

            _logger.LogInformation($"Start sending to topic: {_kafkaOptions.Topic}");

            foreach (var forecast in weatherForecast)
            {
                forecast.Id = ++index;
                
                producer.Produce(_kafkaOptions.Topic, new Message<string, string>
                {
                    Key = forecast.Id.ToString(),
                    Value = JsonSerializer.Serialize(forecast),
                    Timestamp = Timestamp.Default
                });
            }

            _logger.LogInformation($"Finish sending to topic: {_kafkaOptions.Topic}");

            _logger.LogInformation($"Wait in milliseconds: {_kafkaOptions.WaitInMilliseconds}");
            
            await Task.Delay(_kafkaOptions.WaitInMilliseconds, stoppingToken);
        }
    }
}