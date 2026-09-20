using StudioOMS.Clients;
using StudioOMS.Employees;

namespace StudioOMS.Orders.Timing;


public sealed partial class TimingOrder : Order
{
    private TimingOrder() { }
    public static TimingOrder Create(OrderId orderId, ClientId clientId, EmployeeId salespersonId, decimal totalDays, DateTimeOffset createTime)
    {
        if (orderId == OrderId.Empty)
            throw new ArgumentException("订单 Id 不可为空", nameof(orderId));

        if (clientId == ClientId.Empty)
            throw new ArgumentException("客户 Id 不可为空", nameof(clientId));

        if (salespersonId == EmployeeId.Empty)
            throw new ArgumentException("销售员工 Id 不可为空", nameof(salespersonId));

        if (totalDays <= 0)
            throw new ArgumentOutOfRangeException(nameof(totalDays), "总天数必须大于零");


        return new()
        {
            Id = orderId,
            ClientId = clientId,
            SalespersonId = salespersonId,
            TotalDays = totalDays,
            CreateAt = createTime
        };
    }
    public static TimingOrder Create(ClientId clientId, EmployeeId salespersonId, decimal totalDays, DateTimeOffset createTime) =>
        Create(OrderId.Create(), clientId, salespersonId, totalDays, createTime);


    public static TimingOrder CreateNow(OrderId orderId, ClientId clientId, EmployeeId salespersonId, decimal totalDays) =>
        Create(orderId, clientId, salespersonId, totalDays, DateTimeOffset.UtcNow);
    public static TimingOrder CreateNow(ClientId clientId, EmployeeId salespersonId, decimal totalDays) =>
        Create(clientId, salespersonId, totalDays, DateTimeOffset.UtcNow);
}