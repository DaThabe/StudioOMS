namespace StudioOMS.Orders.Timing;


public sealed partial record class TimingOrder : Order
{
    private readonly List<TimingConsume> _consumes = [];
    public IReadOnlyList<TimingConsume> Consumes => _consumes.AsReadOnly();


    public ConsumeResult Consume(TimingConsume consume)
    {
        // 未服务
        if (State != OrderState.Servicing)
            return ConsumeResult.NotServicing;
        // 已存在
        if (_consumes.Contains(consume))
            return ConsumeResult.Repeated;
        // 不能操作的员工
        if (!AssignedEmployees.Contains(consume.EmployeeId))
            return ConsumeResult.NotAssigned(consume.EmployeeId);
        // 超过订单额度
        var nextUsedDays = UsedDays + consume.Days;
        if (nextUsedDays > TotalDays)
            return ConsumeResult.Exceed(nextUsedDays, TotalDays);


        UsedDays = nextUsedDays;
        _consumes.Add(consume);


        // 消耗完毕
        if (UsedDays == TotalDays)
        {
            // 标记完成
            var markResult = MarkCompleted(consume.Timestamp);

            if (markResult is not StateChangeResult.SuccessResult)
                throw new InvalidOperationException("订单状态标记异常");
        }

        return ConsumeResult.Success;
    }
}


public abstract record class ConsumeResult
{
    internal static SuccessResult Success { get; } = new();
    internal static NotServicingResult NotServicing { get; } = new();
    internal static RepeatedResult Repeated { get; } = new();
    internal static NotAssignedResult NotAssigned(EmployeeId employeeId) => new()
    {
        EmployeeId = employeeId
    };
    internal static ExceedResult Exceed(decimal currentDays, decimal totalDays) => new()
    {
        CurrentDays = currentDays,
        TotalDays = totalDays
    };


    /// <summary>
    /// 成功
    /// </summary>
    public sealed record class SuccessResult : ConsumeResult
    {
        internal SuccessResult() { }
    }

    /// <summary>
    /// 订单未服务
    /// </summary>
    public sealed record class NotServicingResult : ConsumeResult
    {
        internal NotServicingResult() { }
    }

    /// <summary>
    /// 重复消耗
    /// </summary>
    public sealed record class RepeatedResult : ConsumeResult
    {
        internal RepeatedResult() { }
    }

    /// <summary>
    /// 超过期限
    /// </summary>
    public sealed record class ExceedResult : ConsumeResult
    {
        public required decimal CurrentDays { get; init; }
        public required decimal TotalDays { get; init; }

        internal ExceedResult() { }
    }

    /// <summary>
    /// 无权限
    /// </summary>
    public sealed record class NotAssignedResult : ConsumeResult
    {
        public required EmployeeId EmployeeId { get; init; }

        internal NotAssignedResult() { }
    }
    /// <summary>
    /// 标记完成状态失败
    /// </summary>
    public sealed record class CompleteFailedResult : ConsumeResult
    {
        internal CompleteFailedResult() { }
    }
}