using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Globalization;

namespace StudioOMS.EfCore.ValueConverters;


internal sealed class DateTimeOffset_String_Converter : ValueConverter<DateTimeOffset, string>
{
    public DateTimeOffset_String_Converter() : base
    (
        time => time.UtcDateTime.ToString("yyyy-MM-ddTHH:mm:ss.fffffffZ"),
        str => DateTimeOffset.Parse(str, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal)
    )
    { }
}