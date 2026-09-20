using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using StudioOMS.Employees;

namespace StudioOMS.EfCore.ValueConverters;


internal sealed class EmployeeId_String_Converter : ValueConverter<EmployeeId, string>
{
    public EmployeeId_String_Converter() : base
    (
        id => id.ToString(),
        value => EmployeeId.Parse(value)
    )
    { }
}
