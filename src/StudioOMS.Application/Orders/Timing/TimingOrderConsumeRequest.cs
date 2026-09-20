using StudioOMS.Employees;
using StudioOMS.Messaging;

namespace StudioOMS.Orders.Timing;


public record TimingOrderConsumeRequest : IRequest
{
    public ConsumeId Id { get; init; } = ConsumeId.Create();
    public DateTimeOffset Timestamp { get; init; } = DateTimeOffset.UtcNow;

    public required OrderId OrderId { get; init; }
    public required EmployeeId EmployeeId { get; init; }
    public required decimal ConsuemDays { get; init; }



    internal sealed class Handler(IOrderRepository orderRepository) : IRequestHandler<TimingOrderConsumeRequest>
    {
        public async ValueTask HandleAsync(TimingOrderConsumeRequest request, CancellationToken cancellationToken = default)
        {
            var order = await orderRepository.FindByIdAsync(request.OrderId, cancellationToken)
                ?? throw new InvalidOperationException($"订单 {request.OrderId} 不存在");

            if (order is not TimingOrder timingOrder)
                throw new InvalidOperationException($"订单 {order.Id} 无法扣除天数");

            var consume = TimingOrderConsume.Create(request.Id, request.EmployeeId, request.ConsuemDays, request.Timestamp);
            timingOrder.Consume(consume);

            await orderRepository.SaveAsync(order, cancellationToken);
        }
    }
}