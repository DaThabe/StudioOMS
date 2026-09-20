using StudioOMS.Clients;
using StudioOMS.Employees;
using StudioOMS.Messaging;

namespace StudioOMS.Orders.Timing;


public sealed record class TimingOrderCreateRequest : IRequest
{
    public required ClientId ClientId { get; init; }
    public required EmployeeId SalespersonId { get; init; }
    public required decimal TotalDays { get; init; }


    public OrderId Id { get; init; } = OrderId.Create();
    public DateTimeOffset CreateAt { get; init; } = DateTimeOffset.UtcNow;
    public string Title { get; init; } = "未命名的订单";



    internal sealed class Handler(IOrderRepository orderRepository) : IRequestHandler<TimingOrderCreateRequest>
    {
        public async ValueTask HandleAsync(TimingOrderCreateRequest request, CancellationToken cancellationToken = default)
        {
            var order = TimingOrder.Create(request.Id, request.ClientId, request.SalespersonId, request.TotalDays, request.CreateAt);
            await orderRepository.SaveAsync(order, cancellationToken);
        }
    }
}
