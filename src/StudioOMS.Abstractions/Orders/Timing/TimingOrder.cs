namespace StudioOMS.Orders.Timing;


public sealed record class TimingOrder : Order
{
    private readonly List<TimingConsume> _consumes = [];

    public required decimal TotalDays { get; init; }
    public decimal UsedDays { get; private set; }
    public IReadOnlyList<TimingConsume> Consumes => _consumes.AsReadOnly();


    public ConsumeResult Consume(TimingConsume consume)
    {
        if (State != OrderState.Servicing)
            return ConsumeResult.NotServicing;

        if (_consumes.Contains(consume))
            return ConsumeResult.Repeated;


        var nextUsedDays = UsedDays + consume.Days;
        if (nextUsedDays > TotalDays)
            return ConsumeResult.Exceed(nextUsedDays, TotalDays);

        UsedDays = nextUsedDays;
        _consumes.Add(consume);

        return ConsumeResult.Success;
    }




    private TimingOrder() { }
    public static TimingOrder Create(OrderId orderId, ClientId clientId, EmployeeId salespersonId, decimal totalDays)
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
            TotalDays = totalDays
        };
    }
}