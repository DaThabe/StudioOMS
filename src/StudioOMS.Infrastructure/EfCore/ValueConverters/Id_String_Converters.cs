using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using StudioOMS.Customers;
using StudioOMS.Employees;
using StudioOMS.Orders;
using StudioOMS.Users;

namespace StudioOMS.EfCore.ValueConverters;


internal sealed class UserId_String_Converter : ValueConverter<UserId, string>
{
    public UserId_String_Converter() : base(
        id => id.ToString(),
        value => UserId.Parse(value))
    { }
}


internal sealed class EmployeeId_String_Converter : ValueConverter<EmployeeId, string>
{
    public EmployeeId_String_Converter() : base
    (
        id => id.ToString(),
        value => EmployeeId.Parse(value))
    { }
}


internal sealed class CustomerId_String_Converter : ValueConverter<CustomertId, string>
{
    public CustomerId_String_Converter() : base(
        id => id.ToString(),
        value => CustomertId.Parse(value))
    { }
}

internal sealed class OrderId_String_Converter : ValueConverter<OrderId, string>
{
    public OrderId_String_Converter() : base(
        id => id.ToString(),
        value => OrderId.Parse(value))
    { }
}

internal sealed class ConsumeId_String_Converter : ValueConverter<OrderConsumeId, string>
{
    public ConsumeId_String_Converter() : base(
        id => id.ToString(),
        str => OrderConsumeId.Parse(str))
    { }
}