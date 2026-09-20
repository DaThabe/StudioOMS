using StudioOMS.Orders;
using StudioOMS.Security.Permission;

namespace StudioOMS.Requests.Orders;


public sealed class OrderMarkServicingRequest : IRequest
{
    public required OrderId OrderId { get; init; }


    internal sealed class Handler(IOrderRepository orderRepository) : IRequestHandler<OrderMarkServicingRequest>, IRequirePermissions
    {
        public IReadOnlySet<PermissionType> RequiredPermissions { get; } = PermissionType.Group(PermissionType.OrderManage);

        public async ValueTask HandleAsync(OrderMarkServicingRequest request, CancellationToken cancellationToken = default)
        {
            var order = await orderRepository.FindByIdAsync(request.OrderId, cancellationToken)
                ?? throw new InvalidOperationException($"订单 {request.OrderId} 不存在");

            order.MarkServicingNow();
            await orderRepository.SaveAsync(order, cancellationToken);
        }
    }
}