using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using StudioOMS.Clients;
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


internal sealed class ClientId_String_Converter : ValueConverter<ClientId, string>
{
    public ClientId_String_Converter() : base(
        id => id.ToString(),
        value => ClientId.Parse(value))
    { }
}

internal sealed class OrderId_String_Converter : ValueConverter<OrderId, string>
{
    public OrderId_String_Converter() : base(
        id => id.ToString(),
        value => OrderId.Parse(value))
    { }
}

internal sealed class ConsumeId_String_Converter : ValueConverter<ConsumeId, string>
{
    public ConsumeId_String_Converter() : base(
        id => id.ToString(),
        str => ConsumeId.Parse(str))
    { }
}