namespace StudioOMS.Orders;


/// <summary>
/// 订单
/// </summary>
public abstract partial record class Order
{
    private static readonly Comparer<OrderStateChanged> _stateChangedsComparer = Comparer<OrderStateChanged>.Create((a, b) => a.Timestamp.CompareTo(b.Timestamp));
    private readonly SortedSet<OrderStateChanged> _stateChangeds = new(_stateChangedsComparer);


    /// <summary>
    /// 当前状态
    /// </summary>
    public OrderState State { get; private set; } = OrderState.Waiting;


    /// <summary>
    /// 开始服务
    /// </summary>
    public StateChangeResult MarkServicing(DateTime timestamp) =>
        MarkState(OrderState.Servicing, timestamp);
    public StateChangeResult MarkServicingNow() =>
        MarkServicing(DateTime.Now);


    /// <summary>
    /// 完成服务
    /// </summary>
    protected StateChangeResult MarkCompleted(DateTime timestamp) =>
        MarkState(OrderState.Completed, timestamp);
    public StateChangeResult MarkCompletedNow() =>
        MarkCompleted(DateTime.Now);


    /// <summary>
    /// 暂停服务
    /// </summary>
    public StateChangeResult MarkPaused(DateTime timestamp) =>
        MarkState(OrderState.Paused, timestamp);
    public StateChangeResult MarkPausedNow() =>
        MarkPaused(DateTime.Now);




    private StateChangeResult MarkState(OrderState state, DateTime timestamp)
    {
        var newStateChanged = new OrderStateChanged(state, timestamp);

        // 已存在的状态变化
        if (_stateChangeds.Contains(newStateChanged))
            return StateChangeResult.Changed(newStateChanged.State);

        // 回放验证
        var currentState = OrderState.Waiting;
        OrderStateChanged[] allChangeds = [.. _stateChangeds, newStateChanged];

        foreach (var e in allChangeds.OrderBy(x => x.Timestamp))
        {
            var result = StateTransition(currentState, e.State);
            if (result is not StateChangeResult.SuccessResult) return result;

            currentState = e.State;
        }

        State = newStateChanged.State;
        _stateChangeds.Add(newStateChanged);

        return StateChangeResult.Changed(newStateChanged.State);
    }

    private static StateChangeResult StateTransition(OrderState from, OrderState to)
    {
        if (!AllowedTransitions.TryGetValue(from, out var allowed))
        {
            return StateChangeResult.Unknown(from);
        }

        if (!allowed.Contains(to))
        {
            return StateChangeResult.NotAsExpected(to, allowed);
        }

        return StateChangeResult.Changed(to);
    }

    private static readonly Dictionary<OrderState, OrderState[]> AllowedTransitions = new()
    {
        [OrderState.Waiting] = [OrderState.Servicing, OrderState.Cancelled],
        [OrderState.Servicing] = [OrderState.Paused, OrderState.Completed, OrderState.Terminated],
        [OrderState.Paused] = [OrderState.Servicing, OrderState.Terminated],
        [OrderState.Completed] = [],  // 终态
        [OrderState.Terminated] = [],
        [OrderState.Cancelled] = []
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
/// 订单暂停时段
/// </summary>
public record OrderStateChanged(OrderState State, DateTime Timestamp);



/// <summary>
/// 订单状态标记结果
/// </summary>
public abstract record class StateChangeResult
{
    internal static SuccessResult Changed(OrderState state) => new()
    {
        Marked = state
    };

    internal static NotAsExpectedResult NotAsExpected(OrderState actual, params IEnumerable<OrderState> expect) => new()
    {
        Actual = actual,
        Expects = expect.ToHashSet()
    };
    internal static UnknownResult Unknown(OrderState state) => new()
    {
        State = state
    };


    /// <summary>
    /// 成功
    /// </summary>
    public sealed record class SuccessResult : StateChangeResult
    {
        public required OrderState Marked { get; init; }
        internal SuccessResult() { }
    }

    /// <summary>
    /// 不符合预期
    /// </summary>
    public sealed record class NotAsExpectedResult : StateChangeResult
    {
        public required OrderState Actual { get; init; }
        public required IReadOnlySet<OrderState> Expects { get; init; }
        internal NotAsExpectedResult() { }
    }

    /// <summary>
    /// 未知的状态
    /// </summary>
    public sealed record class UnknownResult : StateChangeResult
    {
        public required OrderState State { get; init; }
        internal UnknownResult() { }
    }
}