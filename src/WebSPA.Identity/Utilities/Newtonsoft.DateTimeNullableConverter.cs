using System;
using Newtonsoft.Json;

namespace MyAlbum.Services.Identity.API.Utilities.Newtonsoft
{
    public class DateTimeNullableConverter : JsonConverter<DateTime?>
    {
        public override DateTime? ReadJson(JsonReader reader, Type objectType, DateTime? existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            string value = reader.Value?.ToString();
            DateTime dateTime;
            return DateTime.TryParse(value, out dateTime) ? dateTime : (DateTime?)null;
        }

        public override void WriteJson(JsonWriter writer, DateTime? value, JsonSerializer serializer)
        {
            if (value.HasValue)
                writer.WriteValue(value.Value.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ssZ"));
        }
    }
}