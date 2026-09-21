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
    public required OrderListItem[] Items { get; init; }
}


public sealed record class OrderListItem
{
    public required string Id { get; init; }
    public required string Title { get; init; }
    public required DateTimeOffset CreateAt { get; init; }

    public required string State { get; init; }

    public required string ClientId { get; init; }
    public required string ClientName { get; init; }
}