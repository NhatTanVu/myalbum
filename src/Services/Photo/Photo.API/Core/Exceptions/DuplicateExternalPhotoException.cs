using System;

namespace MyAlbum.Services.Photo.API.Core.Exceptions
{
    public class DuplicateExternalPhotoException : Exception
    {
        public string Provider { get; set; }
        public string ExternalId { get; set; }

        public DuplicateExternalPhotoException(string provider, string externalId)
            : base($"Duplicate photo: {provider}:{externalId}")
        {
            Provider = provider;
            ExternalId = externalId;
        }
    }
}
