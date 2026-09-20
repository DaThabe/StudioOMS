using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using StudioOMS.Orders;

namespace StudioOMS.EfCore.ValueConverters;


internal sealed class ConsumeId_String_Converter : ValueConverter<ConsumeId, string>
{
    public ConsumeId_String_Converter() : base(
        id => id.ToString(),
        str => ConsumeId.Parse(str))
    {
    }
}