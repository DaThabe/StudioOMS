namespace StudioOMS.Orders;


/// <summary>
/// 订单
/// </summary>
public abstract partial record class Order
{
    private static readonly Comparer<OrderStateChangeEvent> _stateChangedsComparer = Comparer<OrderStateChangeEvent>.Create((a, b) => a.Timestamp.CompareTo(b.Timestamp));
    private readonly SortedSet<OrderStateChangeEvent> _stateChangeds = new(_stateChangedsComparer);


    /// <summary>
    /// 当前状态
    /// </summary>
    public OrderState State { get; private set; } = OrderState.Waiting;


    /// <summary>
    /// 开始服务
    /// </summary>
    public OrderStateChangeResult MarkServicing(DateTime timestamp) =>
        MarkState(OrderState.Servicing, timestamp);
    public OrderStateChangeResult MarkServicingNow() =>
        MarkServicing(DateTime.Now);


    /// <summary>
    /// 完成服务
    /// </summary>
    protected OrderStateChangeResult MarkCompleted(DateTime timestamp) =>
        MarkState(OrderState.Completed, timestamp);
    public OrderStateChangeResult MarkCompletedNow() =>
        MarkCompleted(DateTime.Now);


    /// <summary>
    /// 暂停服务
    /// </summary>
    public OrderStateChangeResult MarkPaused(DateTime timestamp) =>
        MarkState(OrderState.Paused, timestamp);
    public OrderStateChangeResult MarkPausedNow() =>
        MarkPaused(DateTime.Now);


    /// <summary>
    /// 终止服务
    /// </summary>
    public OrderStateChangeResult MarkTerminated(DateTime timestamp) =>
        MarkState(OrderState.Terminated, timestamp);
    public OrderStateChangeResult MarkTerminatedNow() =>
        MarkTerminated(DateTime.Now);

    /// <summary>
    /// 取消服务
    /// </summary>
    public OrderStateChangeResult MarkCancelled(DateTime timestamp)
        => MarkState(OrderState.Cancelled, timestamp);
    public OrderStateChangeResult MarkCancelledNow()
       => MarkCancelled(DateTime.Now);



    private OrderStateChangeResult MarkState(OrderState state, DateTime timestamp)
    {
        var newStateChanged = new OrderStateChangeEvent(state, timestamp);

        // 已存在的状态变化
        if (_stateChangeds.Contains(newStateChanged))
            return OrderStateChangeResult.Changed(newStateChanged.State);

        // 回放验证
        var currentState = OrderState.Waiting;
        OrderStateChangeEvent[] allChangeds = [.. _stateChangeds, newStateChanged];

        foreach (var e in allChangeds.OrderBy(x => x.Timestamp))
        {
            var result = StateTransition(currentState, e.State);
            if (result is not OrderStateChangeResult.ChangedResult) return result;

            currentState = e.State;
        }

        State = newStateChanged.State;
        _stateChangeds.Add(newStateChanged);

        return OrderStateChangeResult.Changed(newStateChanged.State);
    }

    private static OrderStateChangeResult StateTransition(OrderState from, OrderState to)
    {
        if (!AllowedTransitions.TryGetValue(from, out var allowed))
            throw new InvalidOperationException($"无法识别的状态 {from}");

        if (!allowed.Contains(to))
        {
            return OrderStateChangeResult.NotAsExpected(to, allowed);
        }

        return OrderStateChangeResult.Changed(to);
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
/// 订单状态已改变事件
/// </summary>
public record OrderStateChangeEvent(OrderState State, DateTime Timestamp);


/// <summary>
/// 订单状态改变结果
/// </summary>
public abstract record class OrderStateChangeResult
{
    /// <summary>
    /// 已改变
    /// </summary>
    internal static ChangedResult Changed(OrderState state) => new()
    {
        Marked = state
    };
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
    public sealed record class ChangedResult : OrderStateChangeResult
    {
        public required OrderState Marked { get; init; }
        internal ChangedResult() { }
    }

    /// <summary>
    /// 不符合预期
    /// </summary>
    public sealed record class NotAsExpectedResult : OrderStateChangeResult
    {
        public required OrderState Actual { get; init; }
        public required IReadOnlySet<OrderState> Expects { get; init; }
        internal NotAsExpectedResult() { }
    }
}