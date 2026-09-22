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
    /// 服务订单
    /// </summary>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="OrderStateChangeException"></exception>
    public void MarkServicing(DateTimeOffset timestamp) =>
        MarkState(OrderState.Servicing, timestamp);
    /// <summary>
    /// 服务订单
    /// </summary>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="OrderStateChangeException"></exception>
    public void MarkServicingNow() =>
        MarkServicing(DateTime.Now);


    /// <summary>
    /// 暂停订单
    /// </summary>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="OrderStateChangeException"></exception>
    public void MarkPaused(DateTimeOffset timestamp) =>
        MarkState(OrderState.Paused, timestamp);
    /// <summary>
    /// 暂停订单
    /// </summary>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="OrderStateChangeException"></exception>
    public void MarkPausedNow() =>
        MarkPaused(DateTimeOffset.Now);


    /// <summary>
    /// 取消订单
    /// </summary>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="OrderStateChangeException"></exception>
    public void MarkCancelled(DateTimeOffset timestamp)
        => MarkState(OrderState.Cancelled, timestamp);
    /// <summary>
    /// 取消订单
    /// </summary>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="OrderStateChangeException"></exception>
    public void MarkCancelledNow()
       => MarkCancelled(DateTimeOffset.Now);


    /// <summary>
    /// 终止订单
    /// </summary>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="OrderStateChangeException"></exception>
    public void MarkTerminated(DateTimeOffset timestamp) =>
        MarkState(OrderState.Terminated, timestamp);
    /// <summary>
    /// 终止订单
    /// </summary>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="OrderStateChangeException"></exception>
    public void MarkTerminatedNow() =>
        MarkTerminated(DateTimeOffset.Now);


    /// <summary>
    /// 完成订单
    /// </summary>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="OrderStateChangeException"></exception>
    protected void MarkCompleted(DateTimeOffset timestamp) =>
        MarkState(OrderState.Completed, timestamp);
    /// <summary>
    /// 完成订单
    /// </summary>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="OrderStateChangeException"></exception>
    protected void MarkCompletedNow() =>
        MarkCompleted(DateTimeOffset.Now);



    /// <inheritdoc/>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="OrderStateChangeException"></exception>
    private void MarkState(OrderState state, DateTimeOffset timestamp)
    {
        ArgumentException.ThrowIfNotDefined(state);

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
    /// <inheritdoc/>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="OrderStateChangeException"></exception>
    private void AssertStateTransition(OrderState from, OrderState to)
    {
        if (!AllowedTransitions.TryGetValue(from, out var allowed))
            throw new InvalidOperationException($"无法识别的状态 {from}");

        if (!allowed.Contains(to))
            throw new OrderStateChangeException(Id, from, to, allowed.ToHashSet().AsReadOnly());
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