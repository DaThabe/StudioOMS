using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using StudioOMS.Employees;

namespace StudioOMS.EfCore.ValueConverters;

internal sealed class IReadonlySetEmployeeId_String_Converter : ValueConverter<IReadOnlySet<EmployeeId>, string>
{
    public IReadonlySetEmployeeId_String_Converter() : base
    (
        ids => string.Join(',', ids),
        value => value.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(x => EmployeeId.Parse(x)).ToHashSet()
    )
    { }
}