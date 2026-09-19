namespace StudioOMS.Orders.Timing;


public sealed record class TimingConsume : Consume
{
    public required decimal Days { get; init; }


    private TimingConsume() { }
    public static TimingConsume Create(ConsumeId consumeId, EmployeeId employeeId, decimal days, DateTime timestamp)
    {
        if (consumeId == ConsumeId.Empty)
            throw new ArgumentException("划扣 Id 不可为空", nameof(consumeId));

        if (employeeId == EmployeeId.Empty)
            throw new ArgumentException("员工 Id 不可为空", nameof(employeeId));

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

    public static TimingConsume CreateNow(ConsumeId consumeId, EmployeeId employeeId, decimal days) =>
        Create(consumeId, employeeId, days, DateTime.Now);
}