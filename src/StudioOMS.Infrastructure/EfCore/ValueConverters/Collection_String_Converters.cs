using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using StudioOMS.Employees;
using StudioOMS.Orders;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace StudioOMS.EfCore.ValueConverters;


internal sealed class HashSetEmployeeId_String_Converter : ValueConverter<IReadOnlySet<EmployeeId>, string>
{
    public HashSetEmployeeId_String_Converter() : base
    (
        values => JsonSerializer.Serialize(values, CollectionValueConverterJsonSerializerContext.Default.HashSetEmployeeId),
        str => JsonSerializer.Deserialize(str, CollectionValueConverterJsonSerializerContext.Default.HashSetEmployeeId)!
    )
    { }
}


internal sealed class SortedSetOrderStateChange_String_Converter : ValueConverter<IReadOnlyCollection<OrderStateChange>, string>
{
    public SortedSetOrderStateChange_String_Converter() : base
    (
        values => JsonSerializer.Serialize(values, CollectionValueConverterJsonSerializerContext.Default.SortedSetOrderStateChange),
        str => JsonSerializer.Deserialize(str, CollectionValueConverterJsonSerializerContext.Default.SortedSetOrderStateChange)!
    )
    { }
}


[JsonSourceGenerationOptions(
    UseStringEnumConverter = true,
    Converters = [
        typeof(DateTimeOffsetConverter),
        typeof(EmployeeIdConverter),
    ]
)]
[JsonSerializable(typeof(SortedSet<OrderStateChange>))]
[JsonSerializable(typeof(HashSet<EmployeeId>))]
internal partial class CollectionValueConverterJsonSerializerContext : JsonSerializerContext;


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
internal sealed class EmployeeIdConverter : JsonConverter<EmployeeId>
{
    public override EmployeeId Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var value = reader.GetString();
        return EmployeeId.Parse(value!);
    }

    public override void Write(Utf8JsonWriter writer, EmployeeId value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString());
    }
}