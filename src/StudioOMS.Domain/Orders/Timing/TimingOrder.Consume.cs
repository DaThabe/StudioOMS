using StudioOMS.Employees;

namespace StudioOMS.Orders.Timing;


public sealed partial class TimingOrder : Order
{
    private readonly List<TimingOrderConsume> _consumes = [];
    public IReadOnlyList<TimingOrderConsume> Consumes => _consumes.AsReadOnly();


    /// <inheritdoc/>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    /// <exception cref="OrderStateOperationException"></exception>
    /// <exception cref="OrderNotAssignedEmployeeException"></exception>
    /// <exception cref="OrderConsumeTimestampInvalidException"></exception>
    /// <exception cref="TimingOrderConsumeExceedsLimitException"></exception>
    public void Consume(EmployeeId employeeId, decimal days, DateTimeOffset timestamp)
    {
        ArgumentNullException.ThrowIfNull(employeeId);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(days);


        // 状态不允许
        if (State is not OrderState.Servicing)
            throw new OrderStateOperationException(Id, State, OrderStateOperationType.Consume);
        // 不能操作的员工
        if (!AssignedEmployees.Contains(employeeId))
            throw new OrderNotAssignedEmployeeException(Id, employeeId);
        // 时间异常
        if (timestamp < CreateAt)
            throw new OrderConsumeTimestampInvalidException(Id, employeeId, CreateAt, timestamp);
        // 额度异常
        var nextUsedDays = UsedDays + days;
        if (nextUsedDays > TotalDays)
            throw new TimingOrderConsumeExceedsLimitException(Id, employeeId, TotalDays, nextUsedDays);

        // 划扣信息
        var consume = TimingOrderConsume.Create(employeeId, days, timestamp);
        // 更新
        UsedDays = nextUsedDays;
        _consumes.Add(consume);

        // 消耗完毕
        if (UsedDays >= TotalDays) MarkCompleted(consume.Timestamp);
    }

    /// <inheritdoc/>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    /// <exception cref="OrderStateOperationException"></exception>
    /// <exception cref="OrderNotAssignedEmployeeException"></exception>
    /// <exception cref="OrderConsumeTimestampInvalidException"></exception>
    /// <exception cref="TimingOrderConsumeExceedsLimitException"></exception>
    public void ConsumeNow(EmployeeId employeeId, decimal days) =>
        Consume(employeeId, days, DateTimeOffset.Now);
}