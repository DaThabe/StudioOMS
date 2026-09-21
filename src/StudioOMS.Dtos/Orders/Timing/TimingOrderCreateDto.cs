namespace StudioOMS.Orders.Timing;


public sealed class TimingOrderCreateDto
{
    public required string ClientId { get; init; }
    public required string SalespersonId { get; init; }
    public required decimal TotalDays { get; init; }
    public string Title { get; init; } = "未命名订单";
}

public sealed class OrderCreateResult
{
    public required string OrderId { get; init; }
}