using System.Reflection;
using Azure.Messaging.ServiceBus;

namespace PhotoScrapper.Worker;


public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IConfiguration _config;

    public Worker(ILogger<Worker> logger, IConfiguration config)
    {
        _logger = logger;
        _config = config;
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

            // "Delete" the message from the queue
            await args.CompleteMessageAsync(args.Message);
        };

        processor.ProcessErrorAsync += args =>
        {
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
