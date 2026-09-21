using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace StudioOMS.Serializer.Converters;


/// <summary>
/// 转为 ISO 8601 UTC 格式
/// </summary>
internal sealed class DateTimeOffsetConverter : JsonConverter<DateTimeOffset>
{
    public override DateTimeOffset Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var value = reader.GetString();
        return DateTimeOffset.Parse(value!, CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind);
    }

    public override void Write(Utf8JsonWriter writer, DateTimeOffset value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.UtcDateTime.ToString("yyyy-MM-ddTHH:mm:ss.fffffffZ"));
    }
}