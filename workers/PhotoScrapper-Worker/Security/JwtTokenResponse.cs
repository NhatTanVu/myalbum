using System.Text.Json.Serialization;

namespace PhotoScrapper.Worker.Security;

public record JwtTokenResponse(
    [property: JsonPropertyName("token")]
    string Token,

    [property: JsonPropertyName("expiredAt")]
    DateTimeOffset ExpiredAt
);
