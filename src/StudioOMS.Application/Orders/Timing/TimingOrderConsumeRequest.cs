using StudioOMS.Messaging;

namespace StudioOMS.Orders.Timing;

public record TimingOrderConsumeRequest(OrderId OrderId, EmployeeId EmployeeId, ConsumeId ConsumeId, decimal ConsuemDays) : IRequest<ConsumeResult>
{
    internal sealed class Handler(IOrderRepository orderRepository) : IRequestHandler<TimingOrderConsumeRequest, ConsumeResult>
    {
        public async ValueTask<ConsumeResult> HandleAsync(TimingOrderConsumeRequest request, CancellationToken cancellationToken = default)
        {
            var order = await orderRepository.FindByIdAsync(request.OrderId, cancellationToken)
                ?? throw new InvalidOperationException($"订单 {request.OrderId} 不存在");

            if (order is not TimingOrder timingOrder)
                throw new InvalidOperationException($"订单 {order.Id} 无法扣除天数");

            var result = timingOrder.Consume(TimingConsume.CreateNow(request.ConsumeId, request.EmployeeId, request.ConsuemDays));
            await orderRepository.SaveAsync(order, cancellationToken);

            return result;
        }
    }
}