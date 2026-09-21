using StudioOMS.Employees;
using StudioOMS.Orders;
using StudioOMS.Orders.Timing;
using StudioOMS.Security.Permission;

namespace StudioOMS.Requests.Orders.Timing;


public record TimingOrderConsumeRequest : IRequest
{
    public required OrderId OrderId { get; init; }
    public required EmployeeId EmployeeId { get; init; }
    public required decimal ConsuemDays { get; init; }


    public static TimingOrderConsumeRequest FromDto(Guid orderId, TimingOrderConsumeDto dto)
    {
        return new()
        {
            ConsuemDays = dto.Days,
            EmployeeId = EmployeeId.Parse(dto.EmployeeId),
            OrderId = new(orderId)
        };
    }


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