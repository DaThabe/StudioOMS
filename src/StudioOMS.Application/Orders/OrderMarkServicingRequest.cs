using StudioOMS.Messaging;

namespace StudioOMS.Orders;


public readonly record struct OrderMarkServicingRequest(OrderId OrderId) : IRequest<OrderStateChangeResult>
{
    internal sealed class Handler(IOrderRepository orderRepository) : IRequestHandler<OrderMarkServicingRequest, OrderStateChangeResult>
    {
        public async ValueTask<OrderStateChangeResult> HandleAsync(OrderMarkServicingRequest request, CancellationToken cancellationToken = default)
        {
            var order = await orderRepository.FindByIdAsync(request.OrderId, cancellationToken)
                ?? throw new InvalidOperationException($"订单 {request.OrderId} 不存在");

            var result = order.MarkServicingNow();
            await orderRepository.SaveAsync(order, cancellationToken);

            return result;
        }
    }
}