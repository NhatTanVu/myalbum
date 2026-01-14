using System.Text.Json.Serialization;

namespace PhotoScrapper.Worker.Messaging;

public record IngestionMessage
(
    [property: JsonPropertyName("category")]
    string Category,
    [property: JsonPropertyName("photos_per_category")]
    int PhotosPerCategory,
    [property: JsonPropertyName("criteria")]
    string Criteria,
    [property: JsonPropertyName("provider")]
    string Provider
);