using StudioOMS.Employees;
using System.Collections.Frozen;

namespace StudioOMS.Orders;


/// <summary>
/// 订单
/// </summary>
public abstract partial class Order
{
    private readonly SortedSet<OrderStateTransition> _stateTransitions = [];


    /// <summary>
    /// 当前状态
    /// </summary>
    public OrderState State { get; private set; } = OrderState.Waiting;

    /// <summary>
    /// 状态改变历史
    /// </summary>
    public IReadOnlyCollection<OrderStateTransition> StateTransitions => _stateTransitions.AsReadOnly();


    /// <summary>
    /// 服务订单
    /// </summary>
    /// <exception cref="ArgumentNullException" />
    /// <exception cref="OrderStateTransitionNotAllowedException" />
    /// <exception cref="OrderStateTransitionTimestampInvalidException" />
    internal void MarkServicing(EmployeeId employeeId, DateTimeOffset timestamp) =>
        ManualMarkState(employeeId, OrderState.Servicing, timestamp);
    /// <summary>
    /// 暂停订单
    /// </summary>
    /// <exception cref="ArgumentNullException" />
    /// <exception cref="OrderStateTransitionNotAllowedException" />
    /// <exception cref="OrderStateTransitionTimestampInvalidException" />
    internal void MarkPaused(EmployeeId employeeId, DateTimeOffset timestamp) =>
        ManualMarkState(employeeId, OrderState.Paused, timestamp);
    /// <summary>
    /// 取消订单
    /// </summary>
    /// <exception cref="ArgumentNullException" />
    /// <exception cref="OrderStateTransitionNotAllowedException" />
    /// <exception cref="OrderStateTransitionTimestampInvalidException" />
    internal void MarkCancelled(EmployeeId employeeId, DateTimeOffset timestamp)
        => ManualMarkState(employeeId, OrderState.Cancelled, timestamp);
    /// <summary>
    /// 终止订单
    /// </summary>
    /// <exception cref="ArgumentNullException" />
    /// <exception cref="OrderStateTransitionNotAllowedException" />
    /// <exception cref="OrderStateTransitionTimestampInvalidException" />
    internal void MarkTerminated(EmployeeId employeeId, DateTimeOffset timestamp) =>
        ManualMarkState(employeeId, OrderState.Terminated, timestamp);
    /// <summary>
    /// 完成订单
    /// </summary>
    /// <exception cref="ArgumentNullException" />
    /// <exception cref="OrderStateTransitionNotAllowedException" />
    /// <exception cref="OrderStateTransitionTimestampInvalidException" />
    protected void MarkCompleted(DateTimeOffset timestamp) =>
        AutoMarkState(OrderState.Completed, timestamp);


    /// <inheritdoc/>
    /// <exception cref="ArgumentNullException" />
    /// <exception cref="OrderStateTransitionNotAllowedException" />
    /// <exception cref="OrderStateTransitionTimestampInvalidException" />
    private void ManualMarkState(EmployeeId employeeId, OrderState newState, DateTimeOffset timestamp)
    {
        ArgumentNullException.ThrowIfNull(employeeId);

        if (timestamp < CreateAt)
            throw new OrderStateTransitionTimestampInvalidException(Id, employeeId, CreateAt, timestamp);

        MarkState(OrderStateTransition.Manual(State, newState, employeeId, timestamp));
    }
    /// <inheritdoc/>
    /// <exception cref="OrderStateTransitionNotAllowedException" />
    private void AutoMarkState(OrderState state, DateTimeOffset timestamp)
    {
        MarkState(OrderStateTransition.Auto(State, state, timestamp));
    }

    /// <inheritdoc/>
    /// <exception cref="OrderStateTransitionNotAllowedException" />
    private void MarkState(OrderStateTransition stateChanged)
    {
        // 去重
        if (_stateTransitions.Contains(stateChanged))
            return;

        // 回放验证状态
        PlaybackVerification(stateChanged);

        State = stateChanged.From;
        _stateTransitions.Add(stateChanged);
    }
    /// <summary>回放验证是否能添加此改变记录</summary>
    /// <exception cref="OrderStateTransitionNotAllowedException" />
    private bool PlaybackVerification(OrderStateTransition item)
    {
        var currentState = OrderState.Waiting;
        OrderStateTransition[] allChangeds = [.. _stateTransitions, item];

        foreach (var e in allChangeds.OrderBy(x => x.Timestamp))
        {
            if (!currentState.CanConvertTo(e.To, out var alloweds))
                throw new OrderStateTransitionNotAllowedException(Id, State, item.To, alloweds);

            currentState = e.To;
        }
        return true;
    }
}

/// <summary>
/// 订单状态转换
/// </summary>
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