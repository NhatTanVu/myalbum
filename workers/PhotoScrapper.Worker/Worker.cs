using System.Text.Json;
using Azure.Messaging.ServiceBus;
using PhotoScrapper.Worker.Messaging;
using PhotoScrapper.Worker.Providers.Factory;

namespace PhotoScrapper.Worker;


public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IConfiguration _config;
    private readonly IServiceProvider _serviceProvider;

    public Worker(ILogger<Worker> logger, IConfiguration config,
       IServiceProvider serviceProvider)
    {
        _logger = logger;
        _config = config;
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        string connStr = _config["ServiceBus:ConnectionString"] ?? "";
        string queueName = _config["ServiceBus:QueueName"] ?? "";

        if (string.IsNullOrWhiteSpace(connStr) || string.IsNullOrWhiteSpace(queueName))
        {
            _logger.LogError("Missing ServiceBus configuration. Check appsettings.json.");
            return;
        }

        await using var client = new ServiceBusClient(connStr);
        var processor = client.CreateProcessor(queueName, new ServiceBusProcessorOptions
        {
            AutoCompleteMessages = false,   // we will complete manually
            MaxConcurrentCalls = 1          // keep it slow + easy to understand
        });

        processor.ProcessMessageAsync += async args =>
        {
            string body = args.Message.Body.ToString();
            _logger.LogInformation("Received message: {Body}", body);

            var ingesionMessage = JsonSerializer.Deserialize<IngestionMessage>(body);

            if (ingesionMessage is null ||
                string.IsNullOrWhiteSpace(ingesionMessage.Provider) ||
                string.IsNullOrWhiteSpace(ingesionMessage.Category) ||
                string.IsNullOrWhiteSpace(ingesionMessage.Criteria))
            {
                _logger.LogWarning("Invalid ingestion message payload");
                return;
            }

            var factory = _serviceProvider.GetRequiredService<PhotoProviderFactory>();
            var provider = factory.Create(ingesionMessage.Provider);
            await provider.ProcessPhoto(ingesionMessage);
            _logger.LogInformation("Processed message: {Body}", body);

            // "Delete" the message from the queue
            await args.CompleteMessageAsync(args.Message);
        };

        processor.ProcessErrorAsync += args =>
        {
            if (stoppingToken.IsCancellationRequested)
                return Task.CompletedTask;

            _logger.LogError(args.Exception, "Service Bus error");
            return Task.CompletedTask;
        };

        _logger.LogInformation("Starting Service Bus processor on queue: {Queue}", queueName);
        await processor.StartProcessingAsync(stoppingToken);

        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            await Task.Delay(1000, stoppingToken);
        }

        _logger.LogInformation("Stopping Service Bus processor...");
        await processor.StopProcessingAsync(stoppingToken);
        await processor.DisposeAsync();
    }
}
