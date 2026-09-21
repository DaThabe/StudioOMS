namespace StudioOMS.Orders;


public readonly record struct OrderListDto
{
    public static OrderListDto Default => default;


    public int? Skip { get; init; }
    public int? Take { get; init; }
    public string? Types { get; init; }
}

public sealed record class OrderListResult
{
    public required OrderListItemDto[] Items { get; init; }
    public int Total => Items.Length;
}


public sealed record class OrderListItemDto
{
    public required string Id { get; init; }
    public required string Title { get; init; }
    public required DateTimeOffset CreateAt { get; init; }

    public required string State { get; init; }
    public required string Type { get; init; }

    public required string CustomerId { get; init; }
    public required string CustomerName { get; init; }
}