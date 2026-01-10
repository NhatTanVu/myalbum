using System.ComponentModel.DataAnnotations;
using System.IO.Enumeration;
using System.Text.Json;
using PhotoScrapper.Worker.Integration;
using PhotoScrapper.Worker.Messaging;
using PhotoScrapper.Worker.Persistence;
using PhotoScrapper.Worker.Providers.Pexels;

namespace PhotoScrapper.Worker.Providers;

public class PexelsProvider : IPhotoProvider
{
    private readonly ILogger<PexelsProvider> _logger;
    private readonly PexelsApiClient _pexelsApiClient;
    private readonly PhotoApiClient _photoApiClient;
    private static int _multipartBodyLengthLimit;

    public PexelsProvider(ILogger<PexelsProvider> logger, IConfiguration config)
    {
        _logger = logger;
        _pexelsApiClient = new PexelsApiClient(config["Pexels:ApiKey"]);
        _photoApiClient = new PhotoApiClient(config["PhotoApi:Uri"],
            new Security.JwtTokenClient(config));
        _multipartBodyLengthLimit = int.Parse(config["PhotoApi:MultipartBodyLengthLimit"]);
    }

    public async Task ProcessPhoto(IngestionMessage message)
    {
        _logger.LogInformation(
            "Processing category={Category}, photos={Count}, criteria={Criteria}",
            message.Category,
            message.PhotosPerCategory,
            message.Criteria
        );

        var jsonElements = await _pexelsApiClient.SearchAsync(message.Category, message.PhotosPerCategory);
        Photo[] photos = await Task.WhenAll(jsonElements.Select(MapToPhoto));
        foreach (var photo in photos)
        {
            await _photoApiClient.SavePhotoAsync(photo);
        }
    }

    private static string GetFileName(string sourceUrl, string externalId)
    {
        var uri = new Uri(sourceUrl);

        var fileName = Path.GetFileName(uri.AbsolutePath);

        return string.IsNullOrWhiteSpace(fileName)
            ? $"{externalId}.jpg"
            : fileName;
    }

    private static async Task<Photo> MapToPhoto(JsonElement element)
    {
        var id = element.GetProperty("id").GetInt64().ToString();
        var url = element
            .GetProperty("src")
            .GetProperty("original")
            .GetString();
        var alt = element
            .GetProperty("alt")
            .GetString() ?? "";

        if (url is null)
            throw new InvalidOperationException("Photo URL is missing");

        HttpClient httpClient = new HttpClient();

        string[] all_options = ["large2x", "large", "medium", "small"];
        var imageBytes = await httpClient.GetByteArrayAsync(url);
        int index = 0;
        while ((imageBytes.Length > _multipartBodyLengthLimit - 256) && (index < all_options.Length))
        {
            url = element
                .GetProperty("src")
                .GetProperty(all_options[index++])
                .GetString();
            imageBytes = await httpClient.GetByteArrayAsync(url);
        }
        if (index == all_options.Length)
            throw new InvalidDataException("FileToUpload is too large!");

        return new Photo(
            ExternalId: id,
            SourceUrl: url!,
            FileToUpload: imageBytes,
            Name: alt,
            FileName: GetFileName(url!, id)
        );
    }
}