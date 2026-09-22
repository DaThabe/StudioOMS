namespace StudioOMS.Orders;


/// <summary>
/// 订单
/// </summary>
public abstract partial class Order
{
    private static readonly Comparer<OrderStateChange> _stateChangedsComparer = Comparer<OrderStateChange>.Create((a, b) => a.Timestamp.CompareTo(b.Timestamp));
    private readonly SortedSet<OrderStateChange> _stateChangeds = [with(_stateChangedsComparer)];


    /// <summary>
    /// 当前状态
    /// </summary>
    public OrderState State { get; private set; } = OrderState.Waiting;

    /// <summary>
    /// 状态改变历史
    /// </summary>
    public IReadOnlyCollection<OrderStateChange> StateChangeds => _stateChangeds.AsReadOnly();


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
    protected void MarkCompletedNow() =>
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
public record class OrderStateChange(OrderState State, DateTimeOffset Timestamp) : IComparable<OrderStateChange>
{
    public int CompareTo(OrderStateChange? other) =>
        other is null ? 1 : Timestamp.CompareTo(other.Timestamp);
}