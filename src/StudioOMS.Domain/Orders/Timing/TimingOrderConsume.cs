using StudioOMS.Employees;

namespace StudioOMS.Orders.Timing;


public sealed class TimingOrderConsume : OrderConsume
{
    public required decimal Days { get; init; }


    private TimingOrderConsume() { }
    public static TimingOrderConsume Create(ConsumeId consumeId, EmployeeId employeeId, decimal days, DateTimeOffset timestamp)
    {
        if (days <= 0)
            throw new ArgumentOutOfRangeException(nameof(days), "划扣天数必须大于零");

        return new()
        {
            Id = consumeId,
            EmployeeId = employeeId,
            Days = days,
            Timestamp = timestamp,
        };
    }
    public static TimingOrderConsume Create(EmployeeId employeeId, decimal days, DateTimeOffset timestamp) =>
        Create(ConsumeId.Create(), employeeId, days, timestamp);

    public static TimingOrderConsume CreateNow(ConsumeId consumeId, EmployeeId employeeId, decimal days) =>
        Create(consumeId, employeeId, days, DateTimeOffset.Now);
    public static TimingOrderConsume CreateNow(EmployeeId employeeId, decimal days) =>
        Create(employeeId, days, DateTimeOffset.Now);
}