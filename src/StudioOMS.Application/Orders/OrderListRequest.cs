using StudioOMS.Messaging;
using StudioOMS.Security.Permission;

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
