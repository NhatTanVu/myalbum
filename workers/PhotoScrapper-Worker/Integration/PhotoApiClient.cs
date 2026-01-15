using System.Net;
using System.Net.Http.Headers;
using PhotoScrapper.Worker.Persistence;
using PhotoScrapper.Worker.Security;

namespace PhotoScrapper.Worker.Integration;

public class PhotoApiClient
{
    private readonly ILogger<PhotoApiClient> _logger;
    private readonly HttpClient _http;
    private readonly string _photoApiUri;
    private readonly JwtTokenClient _jwtTokenClient;

    public PhotoApiClient(ILogger<PhotoApiClient> logger,
        string photoApiUri, JwtTokenClient jwtTokenClient)
    {
        _logger = logger;
        _http = new HttpClient();
        _photoApiUri = photoApiUri;
        _jwtTokenClient = jwtTokenClient;
    }

    public async Task SavePhotoAsync(Photo photo)
    {
        var token = await _jwtTokenClient.GetTokenAsync();
        _http.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token);

        _logger.LogInformation($"_photoApiUri={_photoApiUri}");
        using var request = new HttpRequestMessage(
            HttpMethod.Post,
            _photoApiUri);
        var content = new MultipartFormDataContent();
        var fileContent = new ByteArrayContent(photo.FileToUpload);
        fileContent.Headers.ContentType =
            new MediaTypeHeaderValue("image/jpeg");
        content.Add(
            fileContent,
            "FileToUpload",
            photo.FileName);
        content.Add(new StringContent(photo.Name), "Name");
        content.Add(new StringContent(photo.ExternalId), "ExternalId");
        content.Add(new StringContent(photo.ExternalProvider), "ExternalProvider");
        content.Add(new StringContent(photo.SourceUrl), "ExternalUrl");
        request.Content = content;

        var response = await _http.SendAsync(request);

        if (response.StatusCode == HttpStatusCode.Conflict)
        {
            // ✅ Duplicate → expected → stop processing
            _logger.LogInformation(
                "Duplicate photo detected: {Provider}:{ExternalId}",
                photo.ExternalProvider,
                photo.ExternalId);
        }

        response.EnsureSuccessStatusCode();
    }
}