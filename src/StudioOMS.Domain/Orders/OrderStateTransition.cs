using StudioOMS.Employees;
using System.Text.Json.Serialization;

namespace StudioOMS.Orders;


/// <summary>
/// 订单状态改变
/// </summary>
public sealed record class OrderStateTransition : IComparable<OrderStateTransition>
{
    public required OrderState From { get; init; }
    public required OrderState To { get; init; }
    public required DateTimeOffset Timestamp { get; init; }
    public required OrderStateChangeType Type { get; init; }
    public EmployeeId? By { get; init; }


    public int CompareTo(OrderStateTransition? other) =>
        other is null ? 1 : Timestamp.CompareTo(other.Timestamp);


    [JsonConstructor]
    private OrderStateTransition() { }

    /// <inheritdoc/>
    /// <exception cref="ArgumentException" />
    internal static OrderStateTransition Auto(OrderState from, OrderState to, DateTimeOffset timestamp)
    {
        ArgumentException.ThrowIfNotDefined(from);

        return new()
        {
            From = from,
            To = to,
            Type = OrderStateChangeType.Auto,
            Timestamp = timestamp
        };
    }

    /// <inheritdoc/>
    /// <exception cref="ArgumentException" />
    /// <exception cref="ArgumentNullException" />
    internal static OrderStateTransition Manual(OrderState state, OrderState to, EmployeeId employeeId, DateTimeOffset timestamp)
    {
        ArgumentException.ThrowIfNotDefined(state);
        ArgumentNullException.ThrowIfNull(employeeId);

        return new()
        {
            From = state,
            To = to,
            Type = OrderStateChangeType.Manual,
            Timestamp = timestamp,
            By = employeeId
        };
    }
}


/// <summary>
/// 订单状态改变原因
/// </summary>
public enum OrderStateChangeType
{
    Auto = 0,
    Manual = 1
}