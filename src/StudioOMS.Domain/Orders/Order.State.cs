using StudioOMS.Employees;
using System.Collections.Frozen;
using System.Diagnostics.CodeAnalysis;

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
    private void MarkState(OrderState state, DateTimeOffset timestamp, EmployeeId employeeId)
    {
        //if(timestamp < CreateAt)
        //    throw new OrderStateChangeTimestampInvalidException(Id, )


        // 构建
        var stateChanged = OrderStateChange
            .Manual(state, employeeId, timestamp);
        // 去重
        if (_stateChangeds.Contains(stateChanged))
            return;

        // 回放验证状态
        PlaybackVerification(stateChanged);

        State = stateChanged.State;
        _stateChangeds.Add(stateChanged);
    }
    [Obsolete]
    private void MarkState(OrderState state, DateTimeOffset timestamp) =>
        MarkState(state, timestamp, EmployeeId.Create());


    // 回放验证是否能添加此改变记录
    private bool PlaybackVerification(OrderStateChange item)
    {
        var currentState = OrderState.Waiting;
        OrderStateChange[] allChangeds = [.. _stateChangeds, item];

        foreach (var e in allChangeds.OrderBy(x => x.Timestamp))
        {
            if (!currentState.CanConvertTo(e.State, out _))
                return false;

            currentState = e.State;
        }

        return true;
    }
}



file static class OrderStateConverter
{
    /// <summary>
    /// 是可以转换到下一个状态
    /// </summary>
    /// <param name="state">当前状态</param>
    /// <param name="target">目标状态</param>
    /// <param name="alloweds">当前状态可接受的所有下一个状态</param>
    public static bool CanConvertTo(this OrderState state, OrderState target, out IReadOnlySet<OrderState> alloweds)
    {
        alloweds = FrozenSet<OrderState>.Empty;

        if (!Enum.IsDefined(state))
            return false;

        if (!AllowedTransitions.TryGetValue(state, out var nextStates))
            return false;

        if (!nextStates.Contains(target))
            return false;

        alloweds = nextStates.AsReadOnly();
        return true;
    }

    public static IReadOnlySet<OrderState> GetNextStates(this OrderState state)
    {
        ArgumentException.ThrowIfNotDefined(state);
        return AllowedTransitions[state].ToHashSet().AsReadOnly();
    }

    /// <summary>
    /// 各个状态可以转换的状态
    /// </summary>
    private static readonly Dictionary<OrderState, HashSet<OrderState>> AllowedTransitions = new()
    {
        [OrderState.Waiting] = [OrderState.Servicing, OrderState.Cancelled],
        [OrderState.Servicing] = [OrderState.Paused, OrderState.Completed, OrderState.Terminated],
        [OrderState.Paused] = [OrderState.Servicing, OrderState.Terminated],
        [OrderState.Completed] = [],
        [OrderState.Terminated] = [],
        [OrderState.Cancelled] = []
    };
}