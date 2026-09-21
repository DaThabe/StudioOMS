using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using StudioOMS.Customers;
using StudioOMS.Employees;
using StudioOMS.Users;

namespace StudioOMS.EfCore.ValueConverters;


internal sealed class EmployeeName_String_Converter : ValueConverter<EmployeeName, string>
{
    public EmployeeName_String_Converter() : base(
        id => id.ToString(),
        str => EmployeeName.From(str))
    { }
}

internal sealed class Username_String_Converter : ValueConverter<Username, string>
{
    public Username_String_Converter() : base(
        id => id.ToString(),
        str => Username.From(str))
    { }
}

internal sealed class CustomerName_String_Converter : ValueConverter<CustomerName, string>
{
    public CustomerName_String_Converter() : base(
        id => id.ToString(),
        str => CustomerName.From(str))
    { }
}