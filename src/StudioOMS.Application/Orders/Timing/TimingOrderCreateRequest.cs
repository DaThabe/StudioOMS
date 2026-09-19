using StudioOMS.Messaging;

namespace StudioOMS.Orders.Timing;


public record TimingOrderCreateRequest(OrderId OrderId, ClientId ClientId, EmployeeId SalespersonId, decimal TotalDays) : IRequest<OrderId>
{
    internal sealed class Handler(IOrderRepository orderRepository) : IRequestHandler<TimingOrderCreateRequest, OrderId>
    {
        public async ValueTask<OrderId> HandleAsync(TimingOrderCreateRequest request, CancellationToken cancellationToken = default)
        {
            var order = TimingOrder.Create(request.OrderId, request.ClientId, request.SalespersonId, request.TotalDays);
            await orderRepository.SaveAsync(order, cancellationToken);

            return order.Id;
        }
    }
}
