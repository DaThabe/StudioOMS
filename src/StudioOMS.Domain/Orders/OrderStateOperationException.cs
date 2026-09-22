using StudioOMS.Employees;

namespace StudioOMS.Orders;

/// <summary>
/// 订单状态操作异常 - 通常是因为状态不符导致的无法操作
/// </summary>
public sealed class OrderStateOperationException : OrderException
{
    public OrderState CurrentState { get; }
    public OrderStateOperationType Operation { get; }


    internal OrderStateOperationException(OrderId orderId, OrderState currentState, OrderStateOperationType operation)
        : base(FormatMessage(orderId, currentState, operation), orderId)
    {
        CurrentState = currentState;
        Operation = operation;
    }



    private static string FormatMessage(OrderId orderId, OrderState currentState, OrderStateOperationType operation)
    {
        return $"订单 {orderId} {GetStateName(currentState)} 不允许 {GetStateOperationName(operation)}";
    }
    

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


public sealed class OrderStateOperationPermissionException : OrderException
{
    public EmployeeId EmployeeId { get; }
    public OrderState State { get; }


    internal OrderStateOperationPermissionException(OrderId orderId, OrderState state, EmployeeId employeeId)
        : base(FormatMessage(orderId, state, employeeId), orderId)
    {
        EmployeeId = employeeId;
        State = state;
    }

    private static string FormatMessage(OrderId orderId, OrderState currentState, EmployeeId employeeId)
    {
        return $"员工 {employeeId} 没有权限更改订单 {orderId} 状态 {GetStateName(currentState)}";
    }
}