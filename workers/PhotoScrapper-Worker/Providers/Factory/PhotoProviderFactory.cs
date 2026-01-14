namespace PhotoScrapper.Worker.Providers.Factory;

public class PhotoProviderFactory
{
    private readonly IServiceProvider _serviceProvider;

    public PhotoProviderFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public IPhotoProvider Create(string providerName)
    {
        switch (providerName.ToLower())
        {
            case "pexels":
                return _serviceProvider.GetRequiredService<PexelsProvider>();
            default:
                throw new NotSupportedException(
                    $"Photo provider '{providerName}' is not supported");
        }
    }
}