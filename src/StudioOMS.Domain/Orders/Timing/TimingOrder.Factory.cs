using StudioOMS.Customers;
using StudioOMS.Employees;

namespace StudioOMS.Orders.Timing;


public sealed partial class TimingOrder : Order
{
    private TimingOrder() { }
    public static TimingOrder Create(OrderId orderId, CustomertId customerId, EmployeeId salespersonId, decimal totalDays, DateTimeOffset createTime)
    {
        if (orderId == OrderId.Empty)
            throw new ArgumentException("订单 Id 不可为空", nameof(orderId));

        if (customerId == CustomertId.Empty)
            throw new ArgumentException("客户 Id 不可为空", nameof(customerId));

        if (salespersonId == EmployeeId.Empty)
            throw new ArgumentException("销售员工 Id 不可为空", nameof(salespersonId));

        if (totalDays <= 0)
            throw new ArgumentOutOfRangeException(nameof(totalDays), "总天数必须大于零");


        return new()
        {
            Id = orderId,
            CustomerId = customerId,
            SalespersonId = salespersonId,
            TotalDays = totalDays,
            CreateAt = createTime
        };
    }
    public static TimingOrder Create(CustomertId customerId, EmployeeId salespersonId, decimal totalDays, DateTimeOffset createTime) =>
        Create(OrderId.Create(), customerId, salespersonId, totalDays, createTime);


    public static TimingOrder CreateNow(OrderId orderId, CustomertId customerId, EmployeeId salespersonId, decimal totalDays) =>
        Create(orderId, customerId, salespersonId, totalDays, DateTimeOffset.UtcNow);
    public static TimingOrder CreateNow(CustomertId customerId, EmployeeId salespersonId, decimal totalDays) =>
        Create(customerId, salespersonId, totalDays, DateTimeOffset.UtcNow);
}