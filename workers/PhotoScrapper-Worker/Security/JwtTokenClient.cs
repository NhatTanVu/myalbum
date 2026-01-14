
using System.Net.Http.Json;

namespace PhotoScrapper.Worker.Security;

public class JwtTokenClient
{
    private readonly HttpClient _http;
    private readonly IConfiguration _config;
    private string? _cachedToken;
    private DateTimeOffset _expiresAt;

    public JwtTokenClient(IConfiguration config)
    {
        _http = new HttpClient();
        _config = config;
    }

    public async Task<string> GetTokenAsync()
    {
        if (_cachedToken != null &&
            DateTimeOffset.UtcNow < _expiresAt.AddMinutes(-1))
        {
            return _cachedToken;
        }

        var formContent = new FormUrlEncodedContent(new Dictionary<string, string>
        {
            ["UserName"] = _config["JwtAuth:UserName"]!,
            ["Password"] = _config["JwtAuth:Password"]!
        });

        var response = await _http.PostAsync(
            _config["JwtAuth:IssuerUri"]! + "/api/JWT/Generate",
            formContent);
        response.EnsureSuccessStatusCode();

        var token =
            await response.Content.ReadFromJsonAsync<JwtTokenResponse>()
            ?? throw new InvalidOperationException("Invalid JWT response");

        _cachedToken = token.Token;
        _expiresAt = token.ExpiredAt;

        return _cachedToken;
    }
}