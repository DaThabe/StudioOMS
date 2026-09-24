using StudioOMS.Employees;

namespace StudioOMS.Orders;

public sealed class OrderNotAssignedEmployeeException : DomainException
{
    public OrderId OrderId { get; }
    public EmployeeId EmployeeId { get; }

    internal OrderNotAssignedEmployeeException(OrderId orderId, EmployeeId employeeId)
        : base(FormatMessage(orderId, employeeId))
    {
        OrderId = orderId;
        EmployeeId = employeeId;
    }


    private static string FormatMessage(OrderId orderId, EmployeeId employeeId)
    {
        return $"订单 [{orderId}] 未分配该员工 [{employeeId}]";
    }
}