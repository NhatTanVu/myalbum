using System.Net.Http.Headers;
using System.Text.Json;

namespace PhotoScrapper.Worker.Providers.Pexels;

public class PexelsApiClient
{
    private readonly HttpClient _http;

    public PexelsApiClient(string apiKey)
    {
        _http = new HttpClient();
        _http.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue(apiKey);
    }

    public async Task<JsonElement[]> SearchAsync(
        string category,
        int limit)
    {
        var perPage = Math.Min(limit, 80);
        var url =
            $"https://api.pexels.com/v1/search" +
            $"?query={category}&per_page={perPage}";
        var response = await _http.GetAsync(url);
        response.EnsureSuccessStatusCode();

        var doc = JsonDocument.Parse(
            await response.Content.ReadAsStringAsync());

        return doc.RootElement
                  .GetProperty("photos")
                  .EnumerateArray()
                  .ToArray();
    }
}