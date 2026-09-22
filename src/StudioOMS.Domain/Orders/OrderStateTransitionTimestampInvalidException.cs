using StudioOMS.Employees;

namespace StudioOMS.Orders;

/// <summary>
/// 订单状态改变时间异常
/// </summary>
public sealed class OrderStateTransitionTimestampInvalidException : OrderException
{
    public EmployeeId EmployeeId { get; }
    public DateTimeOffset CreateAt { get; }
    public DateTimeOffset ChangeAt { get; }


    internal OrderStateTransitionTimestampInvalidException(
        OrderId orderId,
        EmployeeId employeeId,
        DateTimeOffset createAt,
        DateTimeOffset changeAt
    ) : base(FormatMessage(orderId, createAt, changeAt), orderId)
    {
        EmployeeId = employeeId;
        CreateAt = createAt;
        ChangeAt = changeAt;
    }

    private static string FormatMessage(OrderId orderId, DateTimeOffset createAt, DateTimeOffset consumeAt)
    {
        return $"订单 {orderId} 的状态改变时间 {consumeAt.LocalDateTime:yyyy-MM-dd HH:mm:ss} " +
              $"不能早于订单开始时间 {createAt.LocalDateTime:yyyy-MM-dd HH:mm:ss}";
    }
}
