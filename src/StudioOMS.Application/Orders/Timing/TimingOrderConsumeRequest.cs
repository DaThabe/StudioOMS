using StudioOMS.Employees;
using StudioOMS.Messaging;
using StudioOMS.Orders;
using StudioOMS.Permission;

namespace StudioOMS.Orders.Timing;


public record TimingOrderConsumeRequest : IRequest
{
    public required OrderId OrderId { get; init; }
    public required EmployeeId EmployeeId { get; init; }
    public required decimal ConsuemDays { get; init; }


    


    internal sealed class Handler(
            IOrderRepository orderRepository
        ) : IRequestHandler<TimingOrderConsumeRequest>, IAuthorization
    {
        public IReadOnlySet<PermissionType> RequiredPermissions { get; } =
            PermissionType.Group(PermissionType.OrderConsume);

        public async ValueTask HandleAsync(TimingOrderConsumeRequest request,
            CancellationToken cancellationToken = default)
        {
            var entity = await orderRepository.FindByIdAsync(request.OrderId, cancellationToken)
                ?? throw new InvalidOperationException($"订单 {request.OrderId} 不存在");

            if (entity is not TimingOrder timingOrder)
                throw new InvalidOperationException($"订单 {entity.Id} 无法扣除天数");

            var consume = TimingOrderConsume.CreateNow(request.EmployeeId, request.ConsuemDays);
            timingOrder.Consume(consume);

            await orderRepository.SaveAsync(entity, cancellationToken);
        }
    }
}