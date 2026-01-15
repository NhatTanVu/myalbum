using PhotoScrapper.Worker;
using PhotoScrapper.Worker.Providers;
using PhotoScrapper.Worker.Providers.Factory;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddHostedService<Worker>();
        services.AddSingleton<PexelsProvider>();
        services.AddSingleton<PhotoProviderFactory>();
    })
    .Build();

await host.RunAsync();
