using Microsoft.EntityFrameworkCore.ChangeTracking;
using StudioOMS.Employees;

namespace StudioOMS.EfCore.ValueComparers;


internal sealed class HashSetEmployeeId_String_Comparer : ValueComparer<IReadOnlySet<EmployeeId>>
{
    public HashSetEmployeeId_String_Comparer() : base
    (
        (a, b) => a!.SequenceEqual(b!),
        v => v.Aggregate(0, (hash, e) => HashCode.Combine(hash, e.GetHashCode())),
        v => v.ToHashSet()
    )
    { }
}