using PhotoScrapper.Worker.Messaging;

namespace PhotoScrapper.Worker.Providers;
public interface IPhotoProvider
{
    Task ProcessPhoto(IngestionMessage message);
}