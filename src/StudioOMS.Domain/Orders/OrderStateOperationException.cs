using StudioOMS.Exceptions;

namespace StudioOMS.Orders;

/// <summary>
/// 订单状态操作异常 - 通常是因为状态不符导致的无法操作
/// </summary>
public sealed class OrderStateOperationException : StudioOMSException
{
    public OrderId OrderId { get; }
    public OrderState CurrentState { get; }
    public OrderStateOperationType Operation { get; }


    internal OrderStateOperationException(OrderId orderId, OrderState currentState, OrderStateOperationType operation)
        : base(FormatMessage(orderId, currentState, operation))
    {
        OrderId = orderId;
        CurrentState = currentState;
        Operation = operation;
    }



    private static string FormatMessage(OrderId orderId, OrderState currentState, OrderStateOperationType operation)
    {
        return $"订单 {orderId} {GetStateName(currentState)} 不允许 {GetStateOperationName(operation)}";
    }
    private static string GetStateName(OrderState state) => state switch
    {
        OrderState.Waiting => "未开始服务",
        OrderState.Paused => "已暂停",
        OrderState.Completed => "已完成",
        OrderState.Terminated => "已终止",
        OrderState.Cancelled => "已取消",
        _ => "未知状态"
    };

    private static string GetStateOperationName(OrderStateOperationType state) => state switch
    {
        OrderStateOperationType.Consume => "划扣",
        _ => "未知状态"
    };
}

public enum OrderStateOperationType
{
    /// <summary>
    /// 分配员工
    /// </summary>
    AssignedEmployee,

    /// <summary>
    /// 消耗
    /// </summary>
    Consume,
}