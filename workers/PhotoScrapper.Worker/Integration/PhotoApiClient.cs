using System.Net.Http.Headers;
using PhotoScrapper.Worker.Persistence;
using PhotoScrapper.Worker.Security;

namespace PhotoScrapper.Worker.Integration;

public class PhotoApiClient
{
    private readonly HttpClient _http;
    private readonly string _photoApiUri;
    private readonly JwtTokenClient _jwtTokenClient;

    public PhotoApiClient(string photoApiUri, JwtTokenClient jwtTokenClient)
    {
        _http = new HttpClient();
        _photoApiUri = photoApiUri;
        _jwtTokenClient = jwtTokenClient;
    }

    public async Task SavePhotoAsync(Photo photo)
    {
        var token = await _jwtTokenClient.GetTokenAsync();
        _http.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token);

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
        request.Content = content;

        var response = await _http.SendAsync(request);
        response.EnsureSuccessStatusCode();
    }
}