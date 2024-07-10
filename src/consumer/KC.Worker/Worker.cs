using Confluent.Kafka;
using KC.Worker.Options;
using Microsoft.Extensions.Options;

namespace KC.Worker;

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
        _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

        var config = new ConsumerConfig
        {
            BootstrapServers = _kafkaOptions.BootstrapServers,
            GroupId = _kafkaOptions.GroupId,
            AutoOffsetReset = AutoOffsetReset.Latest,
        };

        using var consumer = new ConsumerBuilder<Ignore, string>(config).Build();

        consumer.Subscribe(_kafkaOptions.Topic);

        while (!stoppingToken.IsCancellationRequested)
        {
            var consumeResult = consumer.Consume(stoppingToken);
            
            _logger.LogInformation("Receive data from topic: {topic}, result: {result}", _kafkaOptions.Topic, consumeResult.Message.Value);
        }

        _logger.LogInformation("Worker finishing at: {time}", DateTimeOffset.Now);
    }
}