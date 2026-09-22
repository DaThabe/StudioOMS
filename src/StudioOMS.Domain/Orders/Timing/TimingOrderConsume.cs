using StudioOMS.Employees;

namespace StudioOMS.Orders.Timing;


public sealed class TimingOrderConsume : OrderConsume
{
    public required decimal Days { get; init; }


    private TimingOrderConsume() { }

    /// <inheritdoc/>
    /// <exception cref="ArgumentException"></exception>
    public static TimingOrderConsume Create(OrderConsumeId consumeId, EmployeeId employeeId, decimal days, DateTimeOffset timestamp)
    {
        if (days <= 0)
            throw new ArgumentException("划扣天数必须大于零", nameof(days));

        return new()
        {
            Id = consumeId,
            EmployeeId = employeeId,
            Days = days,
            Timestamp = timestamp,
        };
    }

    /// <inheritdoc/>
    /// <exception cref="ArgumentException"></exception>
    public static TimingOrderConsume Create(EmployeeId employeeId, decimal days, DateTimeOffset timestamp) =>
        Create(OrderConsumeId.Create(), employeeId, days, timestamp);

    /// <inheritdoc/>
    /// <exception cref="ArgumentException"></exception>
    public static TimingOrderConsume CreateNow(OrderConsumeId consumeId, EmployeeId employeeId, decimal days) =>
        Create(consumeId, employeeId, days, DateTimeOffset.Now);

    /// <inheritdoc/>
    /// <exception cref="ArgumentException"></exception>
    public static TimingOrderConsume CreateNow(EmployeeId employeeId, decimal days) =>
        Create(employeeId, days, DateTimeOffset.Now);
}