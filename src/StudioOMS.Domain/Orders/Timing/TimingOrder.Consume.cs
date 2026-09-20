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
            throw new ArgumentException($"无法划扣, 订单 [{consume.Id}] {GetStateName(State)}", nameof(consume));
        // 已存在
        if (_consumes.Contains(consume))
            throw new ArgumentException($"无法划扣, 该划扣 [{consume.Id}] 已存在");
        // 不能操作的员工
        if (!AssignedEmployees.Contains(consume.EmployeeId))
            throw new ArgumentException($"无法划扣, 该员工 [{consume.EmployeeId}] 未服务此订单 [{Id}]");
        // 超过订单额度
        var nextUsedDays = UsedDays + consume.Days;
        if (nextUsedDays > TotalDays)
            throw new ArgumentOutOfRangeException($"无法划扣, 当前消耗 [{nextUsedDays}] 超过订单上线 [{TotalDays}] ");


        UsedDays = nextUsedDays;
        _consumes.Add(consume);

        // 消耗完毕
        if (UsedDays == TotalDays) MarkCompleted(consume.Timestamp);


        // StateName
        static string GetStateName(OrderState state) => state switch
        {
            OrderState.Waiting => "未开始服务",
            OrderState.Paused => "已暂停",
            OrderState.Completed => "已完成",
            OrderState.Terminated => "已终止",
            OrderState.Cancelled => "已取消",
            _ => "当前状态不允许划扣"
        };
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