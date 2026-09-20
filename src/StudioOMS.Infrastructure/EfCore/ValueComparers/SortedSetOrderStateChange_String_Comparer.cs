using Microsoft.EntityFrameworkCore.ChangeTracking;
using StudioOMS.Orders;

namespace StudioOMS.EfCore.ValueComparers;

internal sealed class SortedSetOrderStateChange_String_Comparer : ValueComparer<IReadOnlyCollection<OrderStateChange>>
{
    public SortedSetOrderStateChange_String_Comparer() : base
    (
        (a, b) => a!.SequenceEqual(b!),
        v => v.Aggregate(0, (hash, e) => HashCode.Combine(hash, e.GetHashCode())),
        v => new SortedSet<OrderStateChange>(v)
    )
    { }
}