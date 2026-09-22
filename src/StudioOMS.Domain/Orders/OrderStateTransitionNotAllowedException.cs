using System.Text;

namespace StudioOMS.Orders;

/// <summary>
/// 订单状态改变异常
/// </summary>
public sealed class OrderStateTransitionNotAllowedException : OrderException
{
    public OrderState CurrentState { get; }
    public OrderState NextState { get; }
    public IReadOnlySet<OrderState> AllowedStates { get; }

    internal OrderStateTransitionNotAllowedException(OrderId orderId, OrderState currentState, OrderState nextState, IReadOnlySet<OrderState> allowedStates)
        : base(FormatMessage(orderId, currentState, nextState, allowedStates), orderId)
    {
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
