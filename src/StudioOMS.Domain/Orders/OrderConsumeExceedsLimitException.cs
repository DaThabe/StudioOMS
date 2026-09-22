using StudioOMS.Employees;

namespace StudioOMS.Orders;

/// <summary>
/// 订单划扣超过上限
/// </summary>
public abstract class OrderConsumeExceedsLimitException(
    string message,
    OrderId orderId,
    EmployeeId employeeId
) : OrderConsumeException(message, orderId, employeeId);
