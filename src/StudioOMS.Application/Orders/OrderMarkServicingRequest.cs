using StudioOMS.Messaging;
using StudioOMS.Security.Permission;

namespace StudioOMS.Orders;


public sealed class OrderMarkServicingRequest : IRequest
{
    public required OrderId OrderId { get; init; }


    public static OrderMarkServicingRequest FromOrderId(string orderId)
    {
        return new()
        {
            OrderId = OrderId.Parse(orderId),
        };
    }

    internal sealed class Handler(
            IOrderRepository orderRepository
        ) : IRequestHandler<OrderMarkServicingRequest>, IAuthorization
    {
        public IReadOnlySet<PermissionType> RequiredPermissions { get; } = PermissionType.Group(PermissionType.OrderManage);

        public async ValueTask HandleAsync(OrderMarkServicingRequest request, 
            CancellationToken cancellationToken = default)
        {
            var entity = await orderRepository.FindByIdAsync(request.OrderId, cancellationToken)
                ?? throw new InvalidOperationException($"订单 {request.OrderId} 不存在");

            entity.MarkServicingNow();
            await orderRepository.SaveAsync(entity, cancellationToken);
        }
    }
}