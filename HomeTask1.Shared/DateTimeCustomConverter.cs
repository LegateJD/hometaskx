using System.Globalization;
using Newtonsoft.Json;

namespace HomeTask1.Shared;

public class DateTimeCustomConverter : JsonConverter
{
    private readonly string _format = "yyyy-MM-dd HH:mm:ss";

    public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
    {
        if (value is DateTime dateTime)
        {
            writer.WriteValue(dateTime.ToString(_format, CultureInfo.InvariantCulture));
        }
        else
        {
            throw new JsonSerializationException("Expected DateTime object value.");
        }
    }

    public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
    {
        if (reader.TokenType == JsonToken.String)
        {
            var str = (string)reader.Value!;
            if (!DateTime.TryParseExact(str, _format, CultureInfo.InvariantCulture, DateTimeStyles.None, out var result))
            {
                throw new JsonSerializationException($"Date must be in format {_format}");
            }
            return DateTime.SpecifyKind(result, DateTimeKind.Utc);
        }

        throw new JsonSerializationException("Expected string value.");
    }

    public override bool CanConvert(Type objectType)
    {
        return objectType == typeof(DateTime) || objectType == typeof(DateTime?);
    }
}
