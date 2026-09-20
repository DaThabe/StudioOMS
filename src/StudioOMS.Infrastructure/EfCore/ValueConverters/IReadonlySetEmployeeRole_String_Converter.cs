using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using StudioOMS.Employees;

namespace StudioOMS.EfCore.ValueConverters;

internal sealed class IReadonlySetEmployeeRole_String_Converter : ValueConverter<IReadOnlySet<EmployeeRole>, string>
{
    public IReadonlySetEmployeeRole_String_Converter() : base
    (
        roles => string.Join(',', roles),
        value => value.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(x => Enum.Parse<EmployeeRole>(x)).ToHashSet()
    )
    { }
}