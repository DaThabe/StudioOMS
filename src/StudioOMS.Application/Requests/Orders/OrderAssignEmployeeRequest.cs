using StudioOMS.Employees;
using StudioOMS.Orders;
using StudioOMS.Security.Permission;

namespace StudioOMS.Requests.Orders;


public sealed record class OrderAssignEmployeeRequest : IRequest
{
    public required OrderId Id { get; init; }
    public required EmployeeId EmployeeId { get; init; }


    public static OrderAssignEmployeeRequest FromDto(Guid orderId, OrderAssignEmployeeDto dto)
    {
        return new()
        {
            Id = new(orderId),
            EmployeeId = EmployeeId.Parse(dto.EmployeeId)
        };
    }


    internal sealed class Handler(IOrderRepository orderRepository) : IRequestHandler<OrderAssignEmployeeRequest>, IRequirePermissions
    {
        public IReadOnlySet<PermissionType> RequiredPermissions { get; } = PermissionType.Group(PermissionType.OrderAssign);

        public async ValueTask HandleAsync(OrderAssignEmployeeRequest request, CancellationToken cancellationToken = default)
        {
            var order = await orderRepository.FindByIdAsync(request.Id, cancellationToken)
                ?? throw new InvalidOperationException($"订单 {request.Id} 不存在");

            order.AssignEmployees(request.EmployeeId);

            await orderRepository.SaveAsync(order, cancellationToken);
        }
    }
}
