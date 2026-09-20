namespace StudioOMS.Orders;


public readonly record struct OrderMarkServicingDto
{
    public required Guid OrderId { get; init; }
}