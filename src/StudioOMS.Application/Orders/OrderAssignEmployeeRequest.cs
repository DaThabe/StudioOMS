using StudioOMS.Employees;
using StudioOMS.Messaging;
using StudioOMS.Security.Permission;

namespace StudioOMS.Orders;


public sealed record class OrderAssignEmployeeRequest : IRequest
{
    public required OrderId Id { get; init; }
    public required EmployeeId EmployeeId { get; init; }


    internal sealed class Handler(
            IOrderRepository orderRepository
        ) : IRequestHandler<OrderAssignEmployeeRequest>, IAuthorization
    {
        public IReadOnlySet<PermissionType> RequiredPermissions { get; } = PermissionType.Group(PermissionType.OrderAssign);

        public async ValueTask HandleAsync(OrderAssignEmployeeRequest request,
            CancellationToken cancellationToken = default)
        {
            var entity = await orderRepository.FindByIdAsync(request.Id, cancellationToken)
                ?? throw new InvalidOperationException($"订单 {request.Id} 不存在");

            entity.AssignEmployees(request.EmployeeId);

            await orderRepository.SaveAsync(entity, cancellationToken);
        }
    }
}
