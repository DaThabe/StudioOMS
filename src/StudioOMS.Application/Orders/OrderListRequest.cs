using StudioOMS.Customers;
using StudioOMS.Messaging;
using StudioOMS.Permission;

namespace StudioOMS.Orders;


public sealed class OrderListRequest : IRequest<OrderListResponse>
{
    public required int Skip { get; init; }
    public required int Take { get; init; }
    public IReadOnlySet<OrderType> Types { get; init; } = new HashSet<OrderType>();


    internal sealed class Handler(
            IOrderQuery orderQuery
        ) : IRequestHandler<OrderListRequest, OrderListResponse>, IAuthorization
    {
        public IReadOnlySet<PermissionType> RequiredPermissions { get; } =
            PermissionType.Group(PermissionType.OrderRead);

        public ValueTask<OrderListResponse> HandleAsync(OrderListRequest request,
            CancellationToken cancellationToken = default)
        {
            return orderQuery.QueryAsync(request, cancellationToken);
        }
    }
}

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