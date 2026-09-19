namespace StudioOMS.Orders;


/// <summary>
/// 订单
/// </summary>
public abstract partial record class Order
{
    /// <summary>
    /// 当前状态
    /// </summary>
    public OrderState State { get; private set; } = OrderState.Waiting;


    /// <summary>
    /// 开始服务
    /// </summary>
    public MarkStateResult MarkServicing() =>
        TransitionTo(OrderState.Servicing, OrderState.Waiting, OrderState.Paused);
    /// <summary>
    /// 暂停服务
    /// </summary>
    public MarkStateResult MarkPaused() =>
        TransitionTo(OrderState.Paused, OrderState.Servicing, OrderState.Waiting);
    /// <summary>
    /// 完成服务
    /// </summary>
    protected MarkStateResult MarkCompleted() =>
        TransitionTo(OrderState.Completed, OrderState.Servicing);



    private MarkStateResult TransitionTo(OrderState target, params IEnumerable<OrderState> allowedFrom)
    {
        if (State == target)
            return MarkStateResult.Success(State);

        if (!allowedFrom.Contains(State))
            return MarkStateResult.NotAsExpected(State, allowedFrom);

        State = target;
        return MarkStateResult.Success(State);
    }
}


/// <summary>
/// 订单状态
/// </summary>
public enum OrderState
{
    /// <summary>
    /// 等待开始
    /// </summary>
    Waiting,

    /// <summary>
    /// 服务中
    /// </summary>
    Servicing,

    /// <summary>
    /// 已暂停
    /// </summary>
    Paused,

    /// <summary>
    /// 已完成
    /// </summary>
    Completed,

    /// <summary>
    /// 已终止
    /// </summary>
    Terminated,

    /// <summary>
    /// 已取消
    /// </summary>
    Cancelled
}



/// <summary>
/// 订单状态标记结果
/// </summary>
public abstract record class MarkStateResult
{
    internal static SuccessResult Success(OrderState state) => new()
    {
        Marked = state
    };

    internal static NotAsExpectedResult NotAsExpected(OrderState actual, params IEnumerable<OrderState> expect) => new()
    {
        Actual = actual,
        Expects = expect.ToHashSet()
    };


    /// <summary>
    /// 成功
    /// </summary>
    public sealed record class SuccessResult : MarkStateResult
    {
        public required OrderState Marked { get; init; }
        internal SuccessResult() { }
    }

    /// <summary>
    /// 不符合预期
    /// </summary>
    public sealed record class NotAsExpectedResult : MarkStateResult
    {
        public required OrderState Actual { get; init; }
        public required IReadOnlySet<OrderState> Expects { get; init; }
        internal NotAsExpectedResult() { }
    }
}