using StudioOMS.Employees;

namespace StudioOMS.Orders;

/// <summary>
/// 订单状态改变
/// </summary>
public sealed record class OrderStateChange : IComparable<OrderStateChange>
{
    public required OrderState State { get; init; }
    public required DateTimeOffset Timestamp { get; init; }
    public required OrderStateChangeType Type { get; init; }
    public EmployeeId? By { get; init; }


    public int CompareTo(OrderStateChange? other) =>
        other is null ? 1 : Timestamp.CompareTo(other.Timestamp);



    private OrderStateChange() { }

    /// <inheritdoc/>
    /// <exception cref="ArgumentException" />
    internal static OrderStateChange Auto(OrderState state, DateTimeOffset timestamp)
    {
        ArgumentException.ThrowIfNotDefined(state);

        return new()
        {
            State = state,
            Type = OrderStateChangeType.Auto,
            Timestamp = timestamp
        };
    }
    /// <inheritdoc/>
    /// <exception cref="ArgumentException" />
    internal static OrderStateChange AutoNow(OrderState state) =>
        Auto(state, DateTimeOffset.Now);


    /// <inheritdoc/>
    /// <exception cref="ArgumentException" />
    /// <exception cref="ArgumentNullException" />
    internal static OrderStateChange Manual(OrderState state, EmployeeId employeeId, DateTimeOffset timestamp)
    {
        ArgumentException.ThrowIfNotDefined(state);
        ArgumentNullException.ThrowIfNull(employeeId);

        return new()
        {
            State = state,
            Type = OrderStateChangeType.Manual,
            Timestamp = timestamp,
            By = employeeId
        };
    }
    /// <inheritdoc/>
    /// <exception cref="ArgumentException" />
    /// <exception cref="ArgumentNullException" />
    internal static OrderStateChange ManualNow(OrderState state, EmployeeId employeeId) =>
        Manual(state, employeeId, DateTimeOffset.Now);

}


/// <summary>
/// 订单状态改变原因
/// </summary>
public enum OrderStateChangeType
{
    Auto = 0,
    Manual = 1
}