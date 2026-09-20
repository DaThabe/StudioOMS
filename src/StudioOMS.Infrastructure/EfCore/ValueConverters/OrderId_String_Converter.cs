using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using StudioOMS.Orders;

namespace StudioOMS.EfCore.ValueConverters;


internal sealed class OrderId_String_Converter : ValueConverter<OrderId, string>
{
    public OrderId_String_Converter() : base(
        id => id.ToString(),
        value => OrderId.Parse(value))
    {
    }
}