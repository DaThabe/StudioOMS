using StudioOMS.Employees;

namespace StudioOMS.Orders.Timing;


public sealed class TimingOrderConsume : OrderConsume
{
    public required decimal Days { get; init; }


    private TimingOrderConsume() { }


    /// <inheritdoc/>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    internal static TimingOrderConsume Create(OrderConsumeId consumeId, EmployeeId employeeId, decimal days, DateTimeOffset timestamp)
    {
        ArgumentNullException.ThrowIfNull(consumeId);
        ArgumentNullException.ThrowIfNull(employeeId);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(days);


        return new()
        {
            Id = consumeId,
            EmployeeId = employeeId,
            Days = days,
            Timestamp = timestamp,
        };
    }

    /// <inheritdoc/>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    internal static TimingOrderConsume Create(EmployeeId employeeId, decimal days, DateTimeOffset timestamp) =>
        Create(OrderConsumeId.Create(), employeeId, days, timestamp);
}