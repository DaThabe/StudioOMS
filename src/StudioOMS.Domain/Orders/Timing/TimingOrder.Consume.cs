using StudioOMS.Employees;

namespace StudioOMS.Orders.Timing;


public sealed partial class TimingOrder : Order
{
    private readonly List<TimingOrderConsume> _consumes = [];
    public IReadOnlyList<TimingOrderConsume> Consumes => _consumes.AsReadOnly();


    public void Consume(TimingOrderConsume consume)
    {
        // 状态不允许
        if (State is not OrderState.Servicing)
            throw new OrderStateOperationException(Id, State, "划扣");
        // 已存在
        if (_consumes.Contains(consume))
            throw new OrderConsumeAlreadyExistsException(Id, consume.Id);
        // 不能操作的员工
        if (!AssignedEmployees.Contains(consume.EmployeeId))
            throw new OrderNotAssignedEmployeeException(Id, consume.EmployeeId);
        // 超过订单额度
        var nextUsedDays = UsedDays + consume.Days;
        if (nextUsedDays > TotalDays)
            throw new TimingOrderConsumeExceedsLimitException(Id, consume.Id, TotalDays, nextUsedDays);


        UsedDays = nextUsedDays;
        _consumes.Add(consume);

        // 消耗完毕
        if (UsedDays == TotalDays) MarkCompleted(consume.Timestamp);
    }
}


public abstract class TimingOrderConsumeResult
{
    /// <summary>
    /// 成功
    /// </summary>
    internal static SuccessResult Success { get; } = new();
    /// <summary>
    /// 状态不允许
    /// </summary>
    internal static StateNotAllowedResultResult StateNotAllowed(OrderState state) => new()
    {
        State = state
    };
    /// <summary>
    /// 重复消耗
    /// </summary>
    internal static RepeatedResult Repeated { get; } = new();
    /// <summary>
    /// 未分配的员工
    /// </summary>
    internal static NotAssignedResult NotAssigned(EmployeeId employeeId) => new()
    {
        EmployeeId = employeeId
    };
    /// <summary>
    /// 超过上线
    /// </summary>
    internal static ExceedResult Exceed(decimal currentDays, decimal totalDays) => new()
    {
        CurrentDays = currentDays,
        TotalDays = totalDays
    };


    /// <summary>
    /// 成功
    /// </summary>
    public sealed class SuccessResult : TimingOrderConsumeResult
    {
        internal SuccessResult() { }
        public override string ToString() => "划扣成功";
    }

    /// <summary>
    /// 状态不允许
    /// </summary>
    public sealed class StateNotAllowedResultResult : TimingOrderConsumeResult
    {
        public required OrderState State { get; init; }

        internal StateNotAllowedResultResult() { }
        public override string ToString() => ToString(State);


        private static string ToString(OrderState state) => state switch
        {
            OrderState.Waiting => "订单还未开始服务，无法划扣",
            OrderState.Paused => "订单已暂停，无法划扣",
            OrderState.Completed => "订单已完成，无法划扣",
            OrderState.Terminated => "订单已终止，无法划扣",
            OrderState.Cancelled => "订单已取消，无法划扣",
            _ => "订单当前状态不允许划扣"
        };
    }

    /// <summary>
    /// 重复消耗
    /// </summary>
    public sealed class RepeatedResult : TimingOrderConsumeResult
    {
        internal RepeatedResult() { }
        public override string ToString() => "划扣失败, 重复划扣";
    }

    /// <summary>
    /// 超过期限
    /// </summary>
    public sealed class ExceedResult : TimingOrderConsumeResult
    {
        public required decimal CurrentDays { get; init; }
        public required decimal TotalDays { get; init; }

        internal ExceedResult() { }
        public override string ToString() => $"划扣失败, 超过上限, 最大:{TotalDays}, 当前:{CurrentDays}";
    }

    /// <summary>
    /// 无权限
    /// </summary>
    public sealed class NotAssignedResult : TimingOrderConsumeResult
    {
        public required EmployeeId EmployeeId { get; init; }

        internal NotAssignedResult() { }
        public override string ToString() => $"划扣失败, 员工 [{EmployeeId}] 未服务该订单";
    }
}