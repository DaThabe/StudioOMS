using StudioOMS.Messaging;

namespace StudioOMS.Orders.Timing;

public record TimingOrderConsumeRequest(OrderId OrderId, EmployeeId EmployeeId, ConsumeId ConsumeId, decimal ConsuemDays, DateTime Timestamp) : IRequest<TimingOrderConsumeResult>
{
    internal sealed class Handler(IOrderRepository orderRepository) : IRequestHandler<TimingOrderConsumeRequest, TimingOrderConsumeResult>
    {
        public async ValueTask<TimingOrderConsumeResult> HandleAsync(TimingOrderConsumeRequest request, CancellationToken cancellationToken = default)
        {
            var order = await orderRepository.FindByIdAsync(request.OrderId, cancellationToken)
                ?? throw new InvalidOperationException($"订单 {request.OrderId} 不存在");

            if (order is not TimingOrder timingOrder)
                throw new InvalidOperationException($"订单 {order.Id} 无法扣除天数");

            var result = timingOrder.Consume(TimingConsume.Create(request.ConsumeId, request.EmployeeId, request.ConsuemDays, request.Timestamp));
            await orderRepository.SaveAsync(order, cancellationToken);

            return result;
        }
    }
}