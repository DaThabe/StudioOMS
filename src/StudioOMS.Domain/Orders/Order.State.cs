namespace StudioOMS.Orders;


/// <summary>
/// 订单
/// </summary>
public abstract partial class Order
{
    private static readonly Comparer<OrderStateChange> _stateChangedsComparer = Comparer<OrderStateChange>.Create((a, b) => a.Timestamp.CompareTo(b.Timestamp));
    private readonly SortedSet<OrderStateChange> _stateChangeds = new(_stateChangedsComparer);


    /// <summary>
    /// 当前状态
    /// </summary>
    public OrderState State { get; private set; } = OrderState.Waiting;


    /// <summary>
    /// 开始服务
    /// </summary>
    public void MarkServicing(DateTimeOffset timestamp) =>
        MarkState(OrderState.Servicing, timestamp);
    public void MarkServicingNow() =>
        MarkServicing(DateTime.Now);


    /// <summary>
    /// 完成服务
    /// </summary>
    protected void MarkCompleted(DateTimeOffset timestamp) =>
        MarkState(OrderState.Completed, timestamp);
    public void MarkCompletedNow() =>
        MarkCompleted(DateTimeOffset.Now);


    /// <summary>
    /// 暂停服务
    /// </summary>
    public void MarkPaused(DateTimeOffset timestamp) =>
        MarkState(OrderState.Paused, timestamp);
    public void MarkPausedNow() =>
        MarkPaused(DateTimeOffset.Now);


    /// <summary>
    /// 终止服务
    /// </summary>
    public void MarkTerminated(DateTimeOffset timestamp) =>
        MarkState(OrderState.Terminated, timestamp);
    public void MarkTerminatedNow() =>
        MarkTerminated(DateTimeOffset.Now);

    /// <summary>
    /// 取消服务
    /// </summary>
    public void MarkCancelled(DateTimeOffset timestamp)
        => MarkState(OrderState.Cancelled, timestamp);
    public void MarkCancelledNow()
       => MarkCancelled(DateTimeOffset.Now);



    private void MarkState(OrderState state, DateTimeOffset timestamp)
    {
        var newStateChanged = new OrderStateChange(state, timestamp);

        // 已存在的状态变化
        if (_stateChangeds.Contains(newStateChanged))
            return;

        // 回放验证
        var currentState = OrderState.Waiting;
        OrderStateChange[] allChangeds = [.. _stateChangeds, newStateChanged];

        foreach (var e in allChangeds.OrderBy(x => x.Timestamp))
        {
            AssertStateTransition(currentState, e.State);
            currentState = e.State;
        }

        State = newStateChanged.State;
        _stateChangeds.Add(newStateChanged);
    }

    private static void AssertStateTransition(OrderState from, OrderState to)
    {
        if (!AllowedTransitions.TryGetValue(from, out var allowed))
            throw new InvalidOperationException($"无法识别的状态 {from}");

        if (!allowed.Contains(to))
            throw new ArgumentOutOfRangeException(nameof(to), $"状态 [{to}] 下一个状态只能在 [{string.Join(',', allowed)}] 中");
    }

    private static readonly Dictionary<OrderState, OrderState[]> AllowedTransitions = new()
    {
        [OrderState.Waiting] = [OrderState.Servicing, OrderState.Cancelled],
        [OrderState.Servicing] = [OrderState.Paused, OrderState.Completed, OrderState.Terminated],
        [OrderState.Paused] = [OrderState.Servicing, OrderState.Terminated],
        [OrderState.Completed] = [],
        [OrderState.Terminated] = [],
        [OrderState.Cancelled] = [OrderState.Waiting]
    };
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
/// 订单状态改变
/// </summary>
public record OrderStateChange(OrderState State, DateTimeOffset Timestamp);


/// <summary>
/// 订单状态改变结果
/// </summary>
public abstract record class OrderStateChangeResult
{
    /// <summary>
    /// 已改变
    /// </summary>
    internal static SuccessResult Success { get; } = new();
    /// <summary>
    /// 不符合预期的状态
    /// </summary>
    internal static NotAsExpectedResult NotAsExpected(OrderState actual, params IEnumerable<OrderState> expect) => new()
    {
        Actual = actual,
        Expects = expect.ToHashSet()
    };


    /// <summary>
    /// 状态已改变
    /// </summary>
    public sealed record class SuccessResult : OrderStateChangeResult
    {
        internal SuccessResult() { }
        public override string ToString() => "状态切换成功";
    }

    /// <summary>
    /// 不符合预期
    /// </summary>
    public sealed record class NotAsExpectedResult : OrderStateChangeResult
    {
        public required OrderState Actual { get; init; }
        public required IReadOnlySet<OrderState> Expects { get; init; }

        internal NotAsExpectedResult() { }
        public override string ToString() => $"状态切换失败, 只能在 [{string.Join(',', Expects)}] 状态下切换, 当前 {Actual}";
    }
}