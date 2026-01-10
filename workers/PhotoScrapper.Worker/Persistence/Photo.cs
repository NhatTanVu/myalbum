namespace PhotoScrapper.Worker.Persistence;

public record Photo(
    string ExternalId,
    string Name,
    string FileName,
    byte[] FileToUpload,
    string SourceUrl
);