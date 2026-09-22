using StudioOMS.Employees;

namespace StudioOMS.Orders;


/// <summary>
/// 订单划扣异常基类
/// </summary>
public abstract class OrderConsumeException(
    string message,
    OrderId orderId,
    EmployeeId employeeId
) : OrderException(message, orderId)
{
    public EmployeeId EmployeeId { get; } = employeeId;
}