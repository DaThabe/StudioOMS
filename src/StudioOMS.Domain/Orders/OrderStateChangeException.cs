using StudioOMS.Employees;
using StudioOMS.Exceptions;
using System.Text;

namespace StudioOMS.Orders;

/// <summary>
/// 订单状态改变异常
/// </summary>
public sealed class OrderStateChangeException : StudioOMSException
{
    public OrderId OrderId { get; }
    public OrderState CurrentState { get; }
    public OrderState NextState { get; }
    public IReadOnlySet<OrderState> AllowedStates { get; }

    internal OrderStateChangeException(OrderId orderId, OrderState currentState, OrderState nextState, IReadOnlySet<OrderState> allowedStates)
        : base(FormatMessage(orderId, currentState, nextState, allowedStates))
    {
        OrderId = orderId;
        CurrentState = currentState;
        NextState = nextState;
        AllowedStates = allowedStates;
    }


    private static string FormatMessage(OrderId orderId, OrderState currentState, OrderState nextState, IReadOnlySet<OrderState> allowedStates)
    {
        StringBuilder sb = new();

        sb.Append($"订单 {orderId} 状态 {currentState} 不能转换为 {nextState}");

        if (allowedStates.Count != 0)
        {
            sb.Append($", 只能在 [{{string.Join(',', allowedStates)}}] 中");
        }

        return sb.ToString();
    }
}


/// <summary>
/// 订单状态改变时间异常
/// </summary>
public sealed class OrderStateChangeTimestampInvalidException : OrderConsumeException
{
    public DateTimeOffset CreateAt { get; }
    public DateTimeOffset ChangeAt { get; }

    internal OrderStateChangeTimestampInvalidException(
        OrderId orderId,
        EmployeeId employeeId,
        DateTimeOffset createAt,
        DateTimeOffset changeAt
    ) : base(FormatMessage(orderId, createAt, changeAt), orderId, employeeId)
    {
        CreateAt = createAt;
        ChangeAt = changeAt;
    }

    private static string FormatMessage(OrderId orderId, DateTimeOffset createAt, DateTimeOffset consumeAt)
    {
        return $"订单 {orderId} 的划扣时间 {consumeAt:yyyy-MM-dd HH:mm:ss} " +
              $"不能早于订单开始时间 {createAt:yyyy-MM-dd HH:mm:ss}";
    }
}
