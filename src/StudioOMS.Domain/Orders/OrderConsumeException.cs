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

/// <summary>
/// 订单划扣超过上限基类
/// </summary>
public abstract class OrderConsumeExceedsLimitException(
    string message,
    OrderId orderId,
    EmployeeId employeeId
) : OrderConsumeException(message, orderId, employeeId);


/// <summary>
/// 订单划扣时间异常
/// </summary>
public sealed class OrderConsumeTimestampInvalidException : OrderConsumeException
{
    public DateTimeOffset CreateAt { get; }
    public DateTimeOffset ConsumeAt { get; }

    internal OrderConsumeTimestampInvalidException(
        OrderId orderId,
        EmployeeId employeeId,
        DateTimeOffset createAt,
        DateTimeOffset consumeAt
    ) : base(FormatMessage(orderId, createAt, consumeAt), orderId, employeeId)
    {
        CreateAt = createAt;
        ConsumeAt = consumeAt;
    }

    private static string FormatMessage(OrderId orderId, DateTimeOffset createAt, DateTimeOffset consumeAt)
    {
        return $"订单 {orderId} 的划扣时间 {consumeAt:yyyy-MM-dd HH:mm:ss} " +
              $"不能早于订单开始时间 {createAt:yyyy-MM-dd HH:mm:ss}";
    }
}