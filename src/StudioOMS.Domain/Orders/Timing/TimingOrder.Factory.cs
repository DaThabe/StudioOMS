namespace StudioOMS.Orders.Timing;


public sealed partial record class TimingOrder : Order
{
    private TimingOrder() { }
    public static TimingOrder Create(OrderId orderId, ClientId clientId, EmployeeId salespersonId, decimal totalDays, DateTime createTime)
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
    public static TimingOrder CreateNow(OrderId orderId, ClientId clientId, EmployeeId salespersonId, decimal totalDays) =>
        Create(orderId, clientId, salespersonId, totalDays, DateTime.UtcNow);
}