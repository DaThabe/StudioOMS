using StudioOMS.Employees;
using StudioOMS.Exceptions;

namespace StudioOMS.Orders;

/// <summary>
/// 订单划扣异常基类
/// </summary>
public abstract class OrderConsumeException(
    string message,
    OrderId orderId,
    EmployeeId employeeId
) : StudioOMSException(message)
{
    public OrderId OrderId { get; } = orderId;
    public EmployeeId EmployeeId { get; } = employeeId;
}


public sealed class OrderConsumeTimestampInvalidException : OrderConsumeException
{
    public DateTimeOffset CreateAt { get; }
    public DateTimeOffset ConsumeAt { get; }

    internal OrderConsumeTimestampInvalidException(
        OrderId orderId,
        EmployeeId employeeId,
        DateTimeOffset ceateAt,
        DateTimeOffset consumeAt
    ) : base(FormatMessage(orderId, employeeId, ceateAt, consumeAt), orderId, employeeId)
    {
        CreateAt = ceateAt;
        ConsumeAt = consumeAt;
    }

    private static string FormatMessage(OrderId orderId, EmployeeId employeeId, DateTimeOffset createAt, DateTimeOffset consumeAt)
    {
        return $"订单 {orderId} 划扣时间 ({consumeAt:yyyy:HH:mm}) 不能小于订单开始时间  ({createAt})";
    }
}
