using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MyAlbum.WebSPA.Core.Utilities
{
    public class DateTimeNullableConverter : JsonConverter<DateTime?>
    {
        public override DateTime? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return (reader.GetString() != null) ? DateTime.Parse(reader.GetString()) : (DateTime?)null;
        }

        public override void Write(Utf8JsonWriter writer, DateTime? value, JsonSerializerOptions options)
        {
            if (value.HasValue)
                writer.WriteStringValue(value.Value.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ssZ"));
        }
    }
}