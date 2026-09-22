using Microsoft.EntityFrameworkCore.ChangeTracking;
using StudioOMS.Orders;

namespace StudioOMS.EfCore.ValueComparers;

internal sealed class SortedSetOrderStateTransition_String_Comparer : ValueComparer<IReadOnlyCollection<OrderStateTransition>>
{
    public SortedSetOrderStateTransition_String_Comparer() : base
    (
        (a, b) => a!.SequenceEqual(b!),
        v => v.Aggregate(0, (hash, e) => HashCode.Combine(hash, e.GetHashCode())),
        v => new SortedSet<OrderStateTransition>(v)
    )
    { }
}