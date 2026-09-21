using StudioOMS.Customers;

namespace StudioOMS.Orders;


public sealed record class OrderListResponse
{
    public required IReadOnlyList<OrderListItem> Items { get; init; }
}

public sealed record class OrderListItem
{
    public required OrderId Id { get; init; }
    public required string Title { get; init; }
    public required OrderType Type { get; init; }
    public required OrderState State { get; init; }
    public required DateTimeOffset CreateAt { get; init; }
    public required CustomertId CustomerId { get; init; }
    public required CustomerName CustomerName { get; init; }
}