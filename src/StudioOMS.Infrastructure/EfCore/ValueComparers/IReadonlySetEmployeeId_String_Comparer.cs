using Microsoft.EntityFrameworkCore.ChangeTracking;
using StudioOMS.Employees;

namespace StudioOMS.EfCore.ValueComparers;

internal sealed class IReadonlySetEmployeeId_String_Comparer : ValueComparer<IReadOnlySet<EmployeeId>>
{
    public IReadonlySetEmployeeId_String_Comparer() : base
    (
        (a, b) => a!.SequenceEqual(b!),
        v => v.Aggregate(0, (hash, e) => HashCode.Combine(hash, e.GetHashCode())),
        v => v.ToHashSet()
    )
    { }
}