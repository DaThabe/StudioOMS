namespace StudioOMS.Orders.Timing;


public sealed class TimingOrderCreateDto
{
    public required Guid OrderId { get; init; }
    public required Guid ClientId { get; init; }
    public required Guid SalespersonId { get; init; }
    public required decimal TotalDays { get; init; }
}