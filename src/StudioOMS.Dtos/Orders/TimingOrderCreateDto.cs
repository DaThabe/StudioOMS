namespace StudioOMS.Orders;


public sealed class TimingOrderCreateDto
{
    public required string CustomerId { get; init; }
    public required string SalespersonId { get; init; }
    public required decimal Price { get; init; }
    public required string Currency { get; init; }
    public required decimal TotalDays { get; init; }
    public required string Title { get; init; }
}

public sealed class OrderCreateResult
{
    public required string OrderId { get; init; }
}